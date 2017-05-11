using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using System.IO;

namespace PRedesign
{
    class NPCEnemy : BasicModel
    {
        #region Fields
        float rotation = 0;
        float maxSpeed = 7;
        float dragPercentage = 0.05f;


        //Steering Behaviours and fields
        FollowPath pathSteering;
        Vector3 currentTarget;
        Vector3[] pathPoints;
        bool endPointExact = false;
        Player player;
        Vector3 tileDimenstions = new Vector3(LevelManager.TileSize, 0, LevelManager.TileSize) / 2;

        //Collision
        SphereCollider collider;
        float colliderRadius = 4.5f;
        SphereMovementChecker movementCollider;
        List<ObjectTag> tagsToCheck = new List<ObjectTag> { ObjectTag.wall }; //obstical and player maybe

        //AI FSM fields
        private string aiFile = "Content / AIFSM / StandardPatrol.xml";
        AiFSM brain;
        string previousState;
        bool loadFromFile = false;

        Vector3[] patrolPoints;
        int patrolIndex = 0;
        float patrolChangeRadius = 3;
        double playerNearDistance = LevelManager.TileSize * 2;
        double playerFarDistance = LevelManager.TileSize * 4;
        float idleDelayTime = 3;
        float currentIdleTime = 0;
        float randomPositionMinDistance = 50;
        float randomWanderChangeRadius = 3;
        Vector3 homeBase = Vector3.Zero;
        double minDistanceToHome = 10;

        //Sound detection
        Vector3 investigatePosition;
        bool investigatingSound;

        //Animation fields
        private float deltaTime;
        private float rotationalSpeed = 2f;
        private float animOrientation = 0f;
        private float spikesScale = 1.25f;

        //Audio
        public AudioEmitterComponent audioEmitterComponent;
        #endregion

        #region Properties
        public Vector3[] PathPoints
        {
            get { return pathPoints; }
            set
            {
                pathPoints = value;
                pathSteering.Targets = pathPoints;
            }
        }

        public Vector3 CurrentTarget
        {
            get { return currentTarget; }
            set
            {
                currentTarget = value;
                PathPoints = NavigationMap.FindPath(position, currentTarget, endPointExact).ToArray();

            }
        }

        public Vector3[] PatrolPoints
        {
            get { return patrolPoints; }
            set
            {
                patrolPoints = value;
                if(patrolPoints.Length != 0)
                    CurrentTarget = patrolPoints[0];

            }
        }

        public string AiFile {
            set {
                aiFile = value;
                try
                {
                    loadFromFile = false;
                    System.IO.Stream stream = TitleContainer.OpenStream(aiFile);
                    XDocument xdoc = XDocument.Load(stream);

                    string startState = xdoc.Root.Attribute("startState").Value;
                    brain = new AiFSM(startState);

                    foreach (XElement variable in xdoc.Root.Element("variables").Elements("variable"))
                        valueSetting(variable.Attribute("name").Value, variable.Attribute("value").Value);

                    foreach (XElement stateElement in xdoc.Root.Elements("state"))
                    {
                        string stateName = stateElement.Attribute("state").Value; //Get state name
                        string updateFunctionName = stateElement.Attribute("updateFunction").Value; // getUpdate fuction name
                        AiState state = new AiState(brain, stateName, getUpdateFunction(updateFunctionName));

                        //Loop through conditions
                        foreach (XElement transitionElement in stateElement.Elements())
                        {
                            string nextState = transitionElement.Attribute("toState").Value;
                            string conditionFunction = transitionElement.Attribute("condition").Value;
                            state.addTransition(nextState, getConditionFunction(conditionFunction));
                        }
                        brain.addState(state);
                    }
                    Console.WriteLine("AILoaded: " + position + " -- Will print twice per ai");
                    loadFromFile = true;
                }
                catch (System.IO.FileNotFoundException)
                {
                    loadFromFile = false;
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    loadFromFile = false;
                }

                if (!loadFromFile)
                {
                    brain = new AiFSM("IDLE");
                    //Setup the states and add them to the fsm
                    AiState PatrolState = new AiState(brain, "PATROL", updatePatrol);
                    PatrolState.addTransition("SEEK", playerNear);
                    AiState SeekState = new AiState(brain, "SEEK", updateSeek);
                    SeekState.addTransition("IDLE", playerFar);
                    AiState IdleState = new AiState(brain, "IDLE", updateIdle);
                    IdleState.addTransition("PATROL", idleTimeUp);
                    IdleState.addTransition("SEEK", playerNear);
                    brain.addState(PatrolState);
                    brain.addState(SeekState);
                    brain.addState(IdleState);
                }
            }
        }
        #endregion

        #region Initialize
        public NPCEnemy(Vector3 startPosition, Model model, Player player) : base(startPosition, model)
        {
            velocity = Vector3.Zero;
            currentTarget = position;
            homeBase = position;
            modelBaseOrientation = 0;

            this.player = player;

            //string aiFile;
            //aiFile = "Content/AIFSM/StandardPatrol.xml"; yes
            //aiFile = "Content/AIFSM/ChasePlayer.xml"; yes
            //aiFile = "Content/AIFSM/GuardLocation.xml"; yes
            //aiFile = "Content/AIFSM/RandomWander.xml"; yes
            AiFile = "Content/AIFSM/StandardPatrol.xml";


            //Set up the path following steering behavior
            pathSteering = new FollowPath(this, pathPoints, 0, false, 3);
            pathSteering.TargetRadius = 0.5f;
            pathSteering.SlowRadius = 5;
            pathSteering.MaxSpeed = maxSpeed;
            pathSteering.MaxAcceleration = maxSpeed * 2;

            collider = new SphereCollider(this, ObjectTag.enemy, colliderRadius);
            collider.PositionOffset = new Vector3(0, -1.5f, 0);
            movementCollider = new SphereMovementChecker(collider, tagsToCheck);

            audioEmitterComponent = new AudioEmitterComponent(this);
            //audioEmitterComponent.addSoundEffect("hover", ContentStore.loadedSounds["hover"]);
            audioEmitterComponent.createSoundEffectInstance("hover", ContentStore.loadedSounds["hover"], true, true, true, 1f);
            //audioEmitterComponent.playSoundEffect("hover", 1f);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            if (!brain.StateJustChanged)
                previousState = brain.CurrentState;
            brain.update(gameTime);

            foreach (Collider col in collider.getCollisions())
            {
                //Detects sounds, does not detect more if already detected one.
                if (!investigatingSound && col.Tag == ObjectTag.sound)
                {
                    //Sound heard
                    investigatePosition = col.GameObject.Position;
                    investigatingSound = true;
                }
            }

            //Animation
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            animateRotation(gameTime);
        }

        private void animateRotation(GameTime gameTime)
        {
            animOrientation += rotationalSpeed * deltaTime;
            //resets to zero + overlap
            if (Math.Abs(animOrientation) > MathHelper.TwoPi)
            {
                float overlap = animOrientation - MathHelper.TwoPi;
                animOrientation = 0f + overlap;
            }
            rotationMatrix = Matrix.CreateRotationY(animOrientation);
            model.Bones["spikes_geo"].Transform = Matrix.CreateScale(spikesScale) * Matrix.CreateRotationY(-animOrientation * 2) * Matrix.CreateTranslation(0f, -10f, 0f);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            if (pathPoints != null && (brain.CurrentState.Equals("PATROL") || brain.CurrentState.Equals("SEEK")))
                if (pathPoints.Count() > 1)
                    ObjectMetaDrawer.RenderPath(pathPoints, Color.Yellow); */
            base.Draw(gameTime);
        }

        public override Matrix GetWorld()
        {
            return scaleMatrix * rotationMatrix * translationMatrix;
        }
        #endregion

        #region Helper Methods
        public void update(SteeringOutput steering, float deltaTime)
        {
            if (steering == null)
            {
                velocity = Vector3.Zero;
            }
            else
            {

                checkMovementCollisions(deltaTime);
                velocity.Y = 0;

                //Update position and rotation
                position += velocity * deltaTime;
                collider.updateColliderPos(position);
                orientation += rotation * deltaTime;

                velocity *= 1 - dragPercentage * dragPercentage;

                //Update the velocity and rotation
                velocity += steering.Velocity * deltaTime;
                rotation += steering.Rotation * deltaTime;

                if (velocity.Length() > maxSpeed)
                    velocity = Vector3.Normalize(velocity) * (maxSpeed);
            }

            rotationMatrix = Matrix.CreateRotationY(orientation + modelBaseOrientation);
            translationMatrix = Matrix.CreateTranslation(position);
            collider.updateColliderPos(position);
        }

        private void checkMovementCollisions(float deltaTime)
        {
            //Check movement directions
            if (!movementCollider.canMoveX(position, velocity * deltaTime))
                velocity.X = -Velocity.X * deltaTime;
            //if (!movementCollider.canMoveY(position, velocity * deltaTime))
            //velocity.Y = 0;
            if (!movementCollider.canMoveZ(position, velocity * deltaTime))
                velocity.Z = -Velocity.Z * deltaTime;
        }

        #endregion

        #region FSM Construction

        /// <summary>
        /// Used to translate the function name into an actual function
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        private Func<bool> getConditionFunction(string functionName)
        {
            switch (functionName)
            {
                case "playerNear": return playerNear;
                case "idleTimeUp": return idleTimeUp;
                case "playerFar": return playerFar;
                case "atHomeBase": return atHomeBase;
                case "heardSound": return heardSound;
                default: return idleTimeUp;
            }
        }

        /// <summary>
        /// Used to translate the method name into an update method
        /// </summary>
        /// <param name="updateName"></param>
        /// <returns></returns>
        private Action<GameTime> getUpdateFunction(string updateName)
        {
            switch (updateName)
            {
                case "updatePatrol": return updatePatrol;
                case "updateSeek": return updateSeek;
                case "updateIdle": return updateIdle;
                case "updateRandomWander": return updateRandomWander;
                case "updateReturnToBase": return updateReturnToBase;
                case "updateInvestigate": return updateInvestigate;
                default: return updateIdle;
            }
        }

        private void valueSetting(string valueName, string value)
        {
            switch (valueName)
            {
                case "patrolChangeRadius":
                    if (stringToFloat(value) != float.MinValue)
                        patrolChangeRadius = stringToFloat(value);
                    break;
                case "playerNearDistance":
                    if (stringToDouble(value) != double.MinValue)
                        playerNearDistance = stringToDouble(value);
                    break;
                case "playerFarDistance":
                    if (stringToDouble(value) != double.MinValue)
                        playerFarDistance = stringToDouble(value);
                    break;
                case "idleDelayTime":
                    if (stringToFloat(value) != float.MinValue)
                        idleDelayTime = stringToFloat(value);
                    break;
                case "randomPositionMinDistance":
                    if (stringToFloat(value) != float.MinValue)
                        randomPositionMinDistance = stringToFloat(value);
                    break;
                case "randomWanderChangeRadius":
                    if (stringToFloat(value) != float.MinValue)
                        randomWanderChangeRadius = stringToFloat(value);
                    break;
                case "homeBase":
                    if (stringToVector3(value) != new Vector3(float.MinValue))
                        homeBase = stringToVector3(value);
                    break;
                case "minDistanceToHome":
                    if (stringToDouble(value) != double.MinValue)
                        minDistanceToHome = stringToDouble(value);
                    break;
                case "maxSpeed":
                    if (stringToFloat(value) != float.MinValue)
                        maxSpeed = stringToFloat(value);
                    break;
                case "dragPercentage":
                    if (stringToFloat(value) != float.MinValue)
                        dragPercentage = stringToFloat(value);
                    break;
                default: break;
            }
        }

        private float stringToFloat(string value)
        {
            try
            {
                return float.Parse(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return float.MinValue;
            }
        }
        private double stringToDouble(string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return double.MinValue;
            }
        }
        private Vector3 stringToVector3(string value)
        {
            string[] values = value.Split(',');
            if (values.Length != 3)
                return new Vector3(float.MinValue);
            float x = stringToFloat(values[0]);
            float y = stringToFloat(values[1]);
            float z = stringToFloat(values[2]);
            if (x == float.MinValue || y == float.MinValue || z == float.MinValue)
                return new Vector3(float.MinValue);
            return new Vector3(x, y, z);
        }
        #endregion

        #region FSM condition Functions 
        public bool playerNear()
        {
            return Vector3.Distance(position, player.Position) < playerNearDistance;
        }

        public bool idleTimeUp()
        {
            return currentIdleTime > idleDelayTime;
        }

        public bool playerFar()
        {
            return Vector3.Distance(position, player.Position) > playerFarDistance;
        }

        public bool atHomeBase()
        {
            return Vector3.Distance(position, homeBase) < minDistanceToHome;
        }

        public bool heardSound()
        {
            return investigatingSound;
        }

        public bool reachedInvestigatePosition()
        {
            return Vector3.Distance(position, investigatePosition) < minDistanceToHome;
        }
        #endregion

        #region FSM update functions
        /// <summary>
        /// Updates the patroling behaviour
        /// </summary>
        /// <param name="deltaTime"></param>
        private void updatePatrol(GameTime gameTime)
        {
            rotationalSpeed = 2f;
            endPointExact = false;
            if (!previousState.Equals(brain.CurrentState))
            {
                try
                {
                    CurrentTarget = patrolPoints[patrolIndex];
                }
                catch (Exception e) {
                    CurrentTarget = position;
                }
            }

            if (patrolPoints.Count() <= patrolIndex)
                return;
             if (NavigationMap.isPositionObstructed(patrolPoints[patrolIndex]))
                 patrolIndex++;
            if (patrolIndex >= patrolPoints.Length)
                patrolIndex = 0;
            if (Vector3.Distance(position, patrolPoints[patrolIndex] + tileDimenstions) < patrolChangeRadius)
            {
                patrolIndex++;
                if (patrolIndex >= patrolPoints.Count())
                    patrolIndex = 0;
                CurrentTarget = patrolPoints[patrolIndex];
            }

            if (pathSteering != null)
            {
                update(pathSteering.getSteering(), deltaTime);
            }
        }

        /// <summary>
        /// Gets the players position and navigates to it
        /// </summary>
        /// <param name="deltaTime"></param>
        private void updateSeek(GameTime gameTime)
        {
            rotationalSpeed = -8f;
            endPointExact = true;
            if (CurrentTarget != player.Position)
                CurrentTarget = player.Position;
            if (pathSteering != null)
            {
                update(pathSteering.getSteering(), deltaTime);
            }
        }

        /// <summary>
        /// Gets a sound's position and navigates to it
        /// </summary>
        /// <param name="deltaTime"></param>
        private void updateInvestigate(GameTime gameTime)
        {
            rotationalSpeed = -4f;
            endPointExact = true;
            if (CurrentTarget != investigatePosition)
            {
                CurrentTarget = investigatePosition;
            }
            if (pathSteering != null)
            {
                update(pathSteering.getSteering(), deltaTime);
            }
        }

        /// <summary>
        /// Increments the idle timer
        /// </summary>
        /// <param name="deltaTime"></param>
        private void updateIdle(GameTime gameTime)
        {
            rotationalSpeed = -0.6f;
            if (!previousState.Equals(brain.CurrentState))
            {
                currentIdleTime = 0;
                velocity = Vector3.Zero;
            }
            currentIdleTime += deltaTime;
        }

        private void updateRandomWander(GameTime gameTime)
        {
            rotationalSpeed = 2f;
            endPointExact = false;
            if (!previousState.Equals(brain.CurrentState))
            {
                CurrentTarget = randomPathablePosition();
            }

            if (Vector3.Distance(position, currentTarget + tileDimenstions) < randomWanderChangeRadius)
            {
                CurrentTarget = randomPathablePosition();
            }

            if (pathSteering != null)
            {
                update(pathSteering.getSteering(), deltaTime);
            }
        }

        private Vector3 randomPathablePosition()
        {
            rotationalSpeed = 2f;
            Random rand = new Random();
            Vector3 testPosition;
            bool positionFound = false;
            if (LevelManager.LevelDepth == 0 || LevelManager.LevelWidth == 0)
                return Vector3.Zero;

            do
            {
                testPosition = new Vector3((float)rand.NextDouble() * LevelManager.LevelWidth, 0, (float)rand.NextDouble() * LevelManager.LevelDepth);
                if (Vector3.Distance(position, testPosition) > randomPositionMinDistance)
                {
                    positionFound = !NavigationMap.isPositionObstructed(testPosition);
                }
            }
            while (!positionFound);

            return testPosition;
        }

        private void updateReturnToBase(GameTime gameTIme)
        {
            rotationalSpeed = 1f;
            if (!previousState.Equals(brain.CurrentState))
            {
                if (homeBase != Vector3.Zero)
                    CurrentTarget = homeBase;
                else
                    CurrentTarget = position;
            }

            if (pathSteering != null)
            {
                update(pathSteering.getSteering(), deltaTime);
            }
        }
        #endregion


    }
}
