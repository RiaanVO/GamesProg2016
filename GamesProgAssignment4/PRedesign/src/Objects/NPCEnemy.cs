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
        float rotation;
        float maxSpeed;
        float dragPercentage;


        //Steering Behaviours and fields
        FollowPath pathSteering;
        Vector3 currentTarget;
        Vector3[] pathPoints;

        Player player;

        //Collision
        SphereCollider collider;
        float colliderRadius = 4.5f;
        SphereMovementChecker movementCollider;
        List<ObjectTag> tagsToCheck = new List<ObjectTag> { ObjectTag.wall, ObjectTag.obstacle, ObjectTag.player};

        //AI FSM fields
        AiFSM brain;
        Vector3[] patrolPoints;
        int patrolIndex = 0;
        float patrolChangeRadius = 1;
        double playerNearDistance = LevelManager.TileSize * 4;
        double playerFarDistance = LevelManager.TileSize * 6;
        float idleDelayTime = 5;
        float currentIdleTime;
        string previousState;
        bool loadFromFile = false;

        //Animation fields
        private float deltaTime;
        private float rotationalSpeed = 1f;
        private float animationRotation;
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
                PathPoints = NavigationMap.FindPath(position, currentTarget).ToArray();
            }
        }

        public Vector3[] PatrolPoints {
            get { return patrolPoints; }
            set
            {
                patrolPoints = value;
                CurrentTarget = patrolPoints[0];
            }
        }
        #endregion

        #region Initialize
        public NPCEnemy(Vector3 startPosition, Model model, Player player) : base(startPosition, ObjectManager.Camera, model)
        {
            velocity = Vector3.Zero;
            currentTarget = position;
            rotation = 0;
            maxSpeed = 5;
            modelBaseOrientation = 0;
            dragPercentage = 0.2f;

            //Set up the path following steering behavior
            pathSteering = new FollowPath(this, pathPoints, 1, false, 1);
            pathSteering.TargetRadius = 0.5f;
            pathSteering.SlowRadius = 5;
            pathSteering.MaxSpeed = maxSpeed;
            pathSteering.MaxAcceleration = maxSpeed * 2;

            this.player = player;

            if (loadFromFile) {
                System.IO.Stream stream = TitleContainer.OpenStream("Content\\AIFSM\\fsm_npc2.xml");
                XDocument xdoc = XDocument.Load(stream);

                string startState = xdoc.Root.Attribute("startState").Value;
                brain = new AiFSM(startState);
                Console.WriteLine(startState);

                foreach (XElement stateElement in xdoc.Root.Elements()) {

                    string updateFunctionName = stateElement.Attribute("state").Value;
                    AiState state = new AiState(brain, stateElement.Attribute("state").Value, getUpdateFunction(updateFunctionName));

                    foreach (XElement transitionElement in stateElement.Elements()) {
                        string nextState = transitionElement.Attribute("toState").Value;
                        string conditionFunction = transitionElement.Attribute("condition").Value;
                        state.addTransition(nextState, getConditionFunction(conditionFunction));
                    }
                    brain.addState(state);
                }
            }
            else
            {
                brain = new AiFSM("PATROL");
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

            collider = new SphereCollider(this, ObjectTag.enemy, colliderRadius);
            collider.PositionOffset = new Vector3(0, -1.5f, 0);
            movementCollider = new SphereMovementChecker(collider, tagsToCheck);
        }
        #endregion

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            previousState = brain.CurrentState;
            brain.update(gameTime);
            
            //Animation
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            animateRotation(gameTime);
        }

        private void animateRotation(GameTime gameTime)
        {
            orientation += rotationalSpeed * deltaTime;
            //resets to zero + overlap
            if (orientation > MathHelper.TwoPi)
            {
                float overlap = orientation - MathHelper.TwoPi;
                orientation = 0f + overlap;
            }
            rotationMatrix = Matrix.CreateRotationY(orientation);
            model.Bones["Spike"].Transform = Matrix.CreateRotationY(-orientation * 2);
        }

        public override void Draw(GameTime gameTime)
        {
            if (pathPoints != null && (brain.CurrentState.Equals("PATROL") || brain.CurrentState.Equals("SEEK")))
                if (pathPoints.Count() > 1)
                    ObjectMetaDrawer.RenderPath(pathPoints, Color.Yellow);
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
                velocity *= 1 - dragPercentage * dragPercentage;

                checkMovementCollisions(deltaTime);
                velocity.Y = 0;

                //Update position and rotation
                position += velocity * deltaTime;
                collider.updateColliderPos(position);
                orientation += rotation * deltaTime;

                //Update the velocity and rotation
                velocity += steering.Velocity * deltaTime;
                rotation += steering.Rotation * deltaTime;

                if (velocity.Length() > maxSpeed)
                    velocity = Vector3.Normalize(velocity) * (maxSpeed);
            }

            rotationMatrix = Matrix.CreateRotationY(orientation + modelBaseOrientation);
            translationMatrix = Matrix.CreateTranslation(position);
        }

        private void checkMovementCollisions(float deltaTime)
        {
            //Check movement directions
            if (!movementCollider.canMoveX(position, velocity * deltaTime))
                velocity.X = 0;
            //if (!movementCollider.canMoveY(position, velocity * deltaTime))
              //  velocity.Y = 0;
            if (!movementCollider.canMoveZ(position, velocity * deltaTime))
                velocity.Z = 0;
        }


        #endregion

        #region FSM Construction

        /// <summary>
        /// Used to translate the function name into an actual function
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        private Func<bool> getConditionFunction(string functionName) {
            switch (functionName) {
                case "playerNear": return playerNear;
                case "idleTimeUp": return idleTimeUp;
                case "playerFar": return playerFar;
                default: return idleTimeUp;
            }
        }

        /// <summary>
        /// Used to translate the method name into an update method
        /// </summary>
        /// <param name="updateName"></param>
        /// <returns></returns>
        private Action<GameTime> getUpdateFunction(string updateName) {
            switch (updateName) {
                case "updatePatrol": return updatePatrol;
                case "updateSeek": return updateSeek;
                case "updateIdle": return updateIdle;
                default: return updateIdle;
            }
        }
        #endregion

        #region FSM condition Functions 
        public bool playerNear() {
            return Vector3.Distance(position, player.Position) < playerNearDistance;
        }

        public bool idleTimeUp() {
            return currentIdleTime > idleDelayTime;
        }

        public bool playerFar() {
            return Vector3.Distance(position, player.Position) > playerFarDistance;
        }
        #endregion

        #region FSM update functions
        /// <summary>
        /// Updates the patroling behaviour
        /// </summary>
        /// <param name="deltaTime"></param>
        private void updatePatrol(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!previousState.Equals(brain.CurrentState))
            {
                
                CurrentTarget = patrolPoints[patrolIndex];
            }

            if (NavigationMap.isPositionObstructed(patrolPoints[patrolIndex]))
                patrolIndex++;

            if (Vector3.Distance(position, patrolPoints[patrolIndex]) < patrolChangeRadius)
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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (CurrentTarget != player.Position)
                CurrentTarget = player.Position;
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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!previousState.Equals(brain.CurrentState))
            {
                currentIdleTime = 0;
            }
            currentIdleTime += deltaTime;
        }
        #endregion
    }
}
