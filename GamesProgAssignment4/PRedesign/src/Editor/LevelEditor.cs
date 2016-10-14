using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PRedesign {

    static class LevelEditor {

        #region Enums
        public enum PaintSelection {
            EMPTY,
            WALL,
            PATH
        }
        #endregion

        #region Fields
        // Editor specific fields
        private static bool isLoaded = false;
        private static SpriteFont editorFont;
        private static GraphicsDevice device;
        private static MouseState mouseState, previousMouseState;
        private static GameScreen screenReference;

        // Level fields
        private static IList<Level> levels;
        private static Level currentEditLevel;
        private static bool isNewLevel = false;

        // Editing area fields
        private static EditorGridElement[,] gridElements;
        private static Texture2D defaultTexture, wallTexture, pathTexture;
        private static PaintSelection selectedPaint = PaintSelection.EMPTY;

        // Editor button fields
        private static IList<UIButton> editorButtons = new List<UIButton>();
        private static IList<UIButton> levelSelectbuttons = new List<UIButton>();
        private static IList<UIButton> enemySelectButtons = new List<UIButton>();
        private static Texture2D buttonTexture, buttonSelectedTexture;
        private static UIButton newButton, loadButton, saveButton, newEnemyButton, browseEnemiesButton, doneEnemyButton, removeEnemyButton;
        private static EditorButtonGroup optionButtonGroup;
        private static EditorOptionButton emptyButton, wallButton, pathButton;
        private static int editorButtonPadding = 5;
        private static bool isLevelSelectLoaded = false;
        private static bool isEnemySelectLoaded = false;

        // Enemy/Node fields
        private static Texture2D enemyTexture, nodeTexture;
        private static bool isPlacingEnemy, isPlacingNode = false;
        private static IList<EditorEnemy> enemyList = new List<EditorEnemy>();
        private static EditorEnemy selectedEnemy;
        private static int enemyLimit = 5;

        // Edit grid element fields
        private static Rectangle gridBounds;
        private static int gridSize = 32;
        private static int elementSize;
        private static int elementOffset = 1;
        private static Vector2 position;
        #endregion

        #region Properties
        public static GraphicsDevice GraphicsDevice {
            get { return device; }
            set { device = value; }
        }

        public static SpriteFont EditorFont {
            get { return editorFont; }
            set { editorFont = value; }
        }

        public static PaintSelection SelectedPaint {
            get { return selectedPaint; }
        }

        public static Texture2D DefaultTexture {
            get { return defaultTexture; }
        }

        public static Texture2D WallTexture {
            get { return wallTexture; }
        }

        public static Texture2D PathTexture {
            get { return pathTexture; }
        }

        public static Texture2D EnemyTexture {
            set { enemyTexture = value; }
        }

        public static Texture2D NodeTexture {
            set { nodeTexture = value; }
        }

        public static bool PlacingEnemy {
            get { return isPlacingEnemy; }
        }

        public static bool PlacingNode {
            get { return isPlacingNode; }
        }

        public static bool Loaded {
            get { return isLoaded; }
        }

        public static GameScreen GameScreen {
            set { screenReference = value; }
        }
        #endregion

        #region Initialisation

        /// <summary>
        /// Loads and initialises the Level Editor
        /// </summary>
        public static void LoadEditor() {

            gridElements = new EditorGridElement[gridSize, gridSize];
            elementSize = (device.Viewport.Width / 2) / gridSize;
            position = new Vector2(10, 10);

            levels = LevelManager.Levels;

            defaultTexture = new Texture2D(device, 1, 1);
            defaultTexture.SetData(new[] { Color.White });

            wallTexture = new Texture2D(device, 1, 1);
            wallTexture.SetData(new[] { Color.Black });

            pathTexture = new Texture2D(device, 1, 1);
            pathTexture.SetData(new[] { Color.Gray });

            buttonTexture = new Texture2D(device, 1, 1);
            buttonTexture.SetData(new[] { Color.DarkRed });

            buttonSelectedTexture = new Texture2D(device, 1, 1);
            buttonSelectedTexture.SetData(new[] { Color.Red });

            InitialiseGrid();
            InitialiseEditorButtons();

            mouseState = Mouse.GetState();

            isLoaded = true;
        }

        /// <summary>
        /// Initialises all the editor buttons
        /// </summary>
        private static void InitialiseEditorButtons() {
            newButton = new UIButton("editor_newButton", new Vector2(450, 20), Vector2.Zero, editorFont, "New Level", Color.White, buttonTexture);
            newButton.Visible = true;
            newButton.Padding = editorButtonPadding;
            newButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(newButton);

            loadButton = new UIButton("editor_loadButton", new Vector2(newButton.Position.X + 100, 20), Vector2.Zero, editorFont, "Load Level", Color.White, buttonTexture);
            loadButton.Visible = true;
            loadButton.Padding = editorButtonPadding;
            loadButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(loadButton);

            saveButton = new UIButton("editor_saveButton", new Vector2(loadButton.Position.X + 100, 20), Vector2.Zero, editorFont, "Save Level", Color.White, buttonTexture);
            saveButton.Visible = true;
            saveButton.Padding = editorButtonPadding;
            saveButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(saveButton);

            newEnemyButton = new UIButton("editor_newEnemyButton", new Vector2(newButton.Position.X, newButton.Position.Y + 50), Vector2.Zero, editorFont, "New Enemy", Color.White, buttonTexture);
            newEnemyButton.Visible = true;
            newEnemyButton.Padding = editorButtonPadding;
            newEnemyButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(newEnemyButton);

            browseEnemiesButton = new UIButton("editor_browseEnemiesButton", new Vector2(newEnemyButton.Position.X + 100, newEnemyButton.Position.Y), Vector2.Zero, editorFont, "Browse Enemies", Color.White, buttonTexture);
            browseEnemiesButton.Visible = false;
            browseEnemiesButton.Padding = editorButtonPadding;
            browseEnemiesButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(browseEnemiesButton);

            doneEnemyButton = new UIButton("editor_doneEnemiesButton", new Vector2(newButton.Position.X, newEnemyButton.Position.Y), Vector2.Zero, editorFont, "Done Creating", Color.White, buttonTexture);
            doneEnemyButton.Visible = false;
            doneEnemyButton.Padding = editorButtonPadding;
            doneEnemyButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(doneEnemyButton);

            removeEnemyButton = new UIButton("editor_doneEnemiesButton", new Vector2(newEnemyButton.Position.X, newEnemyButton.Position.Y + 50), Vector2.Zero, editorFont, "Remove Enemy", Color.White, buttonTexture);
            removeEnemyButton.Visible = false;
            removeEnemyButton.Padding = editorButtonPadding;
            removeEnemyButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(removeEnemyButton);

            optionButtonGroup = new EditorButtonGroup();

            emptyButton = new EditorOptionButton("editor_emptyOptionButton", new Vector2(120, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Clear", Color.White, buttonTexture, buttonSelectedTexture, defaultTexture, 50);
            emptyButton.Visible = true;
            emptyButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            emptyButton.Selected = true;
            optionButtonGroup.AddButton(emptyButton);
            editorButtons.Add(emptyButton);

            wallButton = new EditorOptionButton("editor_wallOptionButton", new Vector2(emptyButton.Position.X + 100, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Wall", Color.White, buttonTexture, buttonSelectedTexture, wallTexture, 50);
            wallButton.Visible = true;
            wallButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            optionButtonGroup.AddButton(wallButton);
            editorButtons.Add(wallButton);

            pathButton = new EditorOptionButton("editor_pathOptionButton", new Vector2(wallButton.Position.X + 100, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Path", Color.White, buttonTexture, buttonSelectedTexture, PathTexture, 50);
            pathButton.Visible = true;
            pathButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            optionButtonGroup.AddButton(pathButton);
            editorButtons.Add(pathButton);
        }
        #endregion

        #region OnClick Handlers

        /// <summary>
        /// OnClick handler for the paint selection buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OptionButtonOnClick(Object sender, EventArgs e) {
            EditorOptionButton button = (EditorOptionButton)sender;
            if (sender == emptyButton) {
                selectedPaint = PaintSelection.EMPTY;
            } else if (sender == wallButton) {
                selectedPaint = PaintSelection.WALL;
            } else if (sender == pathButton) {
                selectedPaint = PaintSelection.PATH;
            }
            optionButtonGroup.ToggleButton(button);
        }

        /// <summary>
        /// OnClick handlers for the level option and enemy/node buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EditorButtonOnClick(Object sender, EventArgs e) {
            if (sender == newButton) {
                browseEnemiesButton.Visible = false;
                UnloadLevelGrid();
                GenerateNewGrid();
                return;
            }

            if (sender == saveButton) {
                //SaveLevel();
                SaveEditorLevel();
                return;
            }

            if (sender == loadButton) {
                if (levels.Count >= 1) {
                    LoadLevelSelectionGrid();
                }
                return;
            }

            if (sender == newEnemyButton) {
                CreateNewEnemyProcess();
                return;
            }

            if (sender == browseEnemiesButton) {
                LoadEnemySelection();
                return;
            }

            if (sender == removeEnemyButton) {
                RemoveSelectedEnemy();
                return;
            }

            if (sender == doneEnemyButton) {
                FinaliseEnemyCreation();
                return;
            }
        }

        /// <summary>
        /// OnClick handler for the buttons during load level selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LevelSelectOnClick(Object sender, EventArgs e) {

            UIButton button = (UIButton)sender;
            
            loadButton.Visible = true;
            saveButton.Visible = true;
            newButton.Visible = true;

            isLevelSelectLoaded = false;
            selectedEnemy = null;

            UnloadLevelGrid();
            LoadLevelGrid(int.Parse(button.tag.ToString()));

            if (enemyList.Count < enemyLimit) {
                newEnemyButton.Visible = true;
            }
            if (enemyList.Count > 0) {
                browseEnemiesButton.Visible = true;
            }

        }

        /// <summary>
        /// OnClick handler for the buttons during enemy selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EnemySelectOnClick(Object sender, EventArgs e) {
            UIButton button = (UIButton)sender;

            loadButton.Visible = true;
            saveButton.Visible = true;
            newButton.Visible = true;
            newEnemyButton.Visible = true;
            browseEnemiesButton.Visible = true;

            isEnemySelectLoaded = false;

            selectedEnemy = enemyList[(int.Parse(button.tag.ToString())) - 1];
            removeEnemyButton.Visible = true;
        }
        #endregion

        #region Main Editor Methods

        #region Grid Initialisation and Loading/Unloading
        /// <summary>
        /// Initialises the 32x32 grid in the Level Editor and sets the bounds
        /// </summary>
        private static void InitialiseGrid() {
            gridBounds = new Rectangle((int)position.X, (int)position.Y, elementSize * gridSize + elementOffset * gridSize, elementSize * gridSize + elementOffset * gridSize);
            GenerateNewGrid();
        }

        /// <summary>
        /// Method to create the Grid Element object
        /// </summary>
        /// <param name="texture"> Texture for the grid element</param>
        /// <param name="selection"> Enum used to set what type of tile to paint </param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private static EditorGridElement CreateNewElement(Texture2D texture, PaintSelection selection, int i, int j) {
            EditorGridElement element = new EditorGridElement(texture, (position.X + elementSize * i) + elementOffset * i, (position.Y + elementSize * j) + elementOffset * j, elementSize, i, j);
            element.Type = selection;
            element.Obstacle = EditorGridElement.ElementObstacle.NONE;
            return element;
        }

        /// <summary>
        /// Generates a new 32x32 grid
        /// </summary>
        private static void GenerateNewGrid() {
            isNewLevel = true;
            selectedEnemy = null;
            enemyList.Clear();
            currentEditLevel = new Level();
            if (levels.Count == 0) {
                currentEditLevel.Id = 0;
            } else {
                currentEditLevel.Id = levels[levels.Count - 1].Id + 1;
            }

            currentEditLevel.Data = new int[gridSize, gridSize];
            currentEditLevel.Enemies = new List<Enemy>();

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    gridElements[i, j] = CreateNewElement(defaultTexture, PaintSelection.EMPTY, i, j);
                }
            }
        }

        /// <summary>
        /// Parses data from a level object and loads the level
        /// </summary>
        /// <param name="id">The number of the level</param>
        private static void LoadLevelGrid(int id) {
            isNewLevel = false;
            currentEditLevel = levels[id - 1];

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    switch (currentEditLevel.Data[i, j]) {
                        case 0:
                            gridElements[i, j] = CreateNewElement(wallTexture, PaintSelection.WALL, i, j);
                            break;
                        case 1:
                            gridElements[i, j] = CreateNewElement(pathTexture, PaintSelection.PATH, i, j);
                            break;
                        default:
                            gridElements[i, j] = CreateNewElement(defaultTexture, PaintSelection.EMPTY, i, j);
                            break;
                    }
                }
            }
            LoadLevelEnemies();
        }

        /// <summary>
        /// Loads the enemy data from a level object
        /// </summary>
        private static void LoadLevelEnemies() {
            if (currentEditLevel.Enemies.Count > 0) {
                foreach (Enemy enemy in currentEditLevel.Enemies) {
                    EditorEnemy editorEnemy = new EditorEnemy(position, elementSize, elementOffset, enemyTexture, nodeTexture, enemy.X, enemy.Y, enemy.PatrolPoints);
                    enemyList.Add(editorEnemy);
                }
            }
        }

        /// <summary>
        /// Removes the grid in preparation to load or generate a new grid
        /// </summary>
        private static void UnloadLevelGrid() {
            gridElements = new EditorGridElement[gridSize, gridSize];
            enemyList = new List<EditorEnemy>();
        }

        #endregion

        #region Level and Enemy Selection Methods
        /// <summary>
        /// Loads the buttons for load level selection and causes some buttons to hide
        /// </summary>
        private static void LoadLevelSelectionGrid() {
            loadButton.Visible = false;
            saveButton.Visible = false;
            newButton.Visible = false;
            newEnemyButton.Visible = false;
            removeEnemyButton.Visible = false;
            browseEnemiesButton.Visible = false;

            levelSelectbuttons.Clear();

            int numLevels = levels.Count;
            double numColumns = Math.Ceiling(numLevels / 4.00);


            int counter = 1;
            for (int y = 0; y <= numColumns - 1; y++) {
                for (int x = 0; x < 4; x++) {
                    UIButton btn = new UIButton("levelSelect_" + counter.ToString(), new Vector2(500 + 60 * x, 50 + 30 * y), Vector2.Zero, editorFont, "Level " + counter.ToString(), Color.White, buttonTexture, counter.ToString());
                    btn.Visible = true;
                    btn.Clicked += new UIButton.ClickHandler(LevelSelectOnClick);
                    levelSelectbuttons.Add(btn);
                    if (counter++ == numLevels) {
                        break;
                    }
                }

            }
            isLevelSelectLoaded = true;
        }

        /// <summary>
        /// Loads the buttons for enemy selection and causes some buttons to hide
        /// </summary>
        private static void LoadEnemySelection() {
            if (enemyList.Count > 0) {
                loadButton.Visible = false;
                saveButton.Visible = false;
                newButton.Visible = false;
                newEnemyButton.Visible = false;
                browseEnemiesButton.Visible = false;
                removeEnemyButton.Visible = false;

                enemySelectButtons.Clear();

                int counter = 1;
                foreach (EditorEnemy enemy in enemyList) {
                    UIButton btn = new UIButton("enemySelect_" + counter.ToString(), new Vector2(500, 60 + (40 * counter)), Vector2.Zero, editorFont, "Enemy " + counter.ToString(), Color.White, buttonTexture, counter.ToString());
                    btn.Visible = true;
                    btn.Clicked += new UIButton.ClickHandler(EnemySelectOnClick);
                    enemySelectButtons.Add(btn);
                    counter++;
                }

                isEnemySelectLoaded = true;
            }
        }
        #endregion

        #region Enemy/Node Methods

        /// <summary>
        /// Removes the current selected enemy
        /// </summary>
        private static void RemoveSelectedEnemy() {
            enemyList.Remove(selectedEnemy);
            selectedEnemy = null;
            removeEnemyButton.Visible = false;
        }


        /// <summary>
        /// Starts the process of creating a new enemy - Requiring an enemy to be placed on the grid
        /// </summary>
        private static void CreateNewEnemyProcess() {
            newButton.Visible = false;
            loadButton.Visible = false;
            saveButton.Visible = false;
            removeEnemyButton.Visible = false;
            optionButtonGroup.Visible(false);
            selectedEnemy = null;
            isPlacingEnemy = true;
            newEnemyButton.Visible = false;
            browseEnemiesButton.Visible = false;
        }

        /// <summary>
        /// Finalises the creation of an enemy and it's nodes
        /// </summary>
        private static void FinaliseEnemyCreation() {
            newButton.Visible = true;
            loadButton.Visible = true;
            saveButton.Visible = true;
            isPlacingNode = false;
            removeEnemyButton.Visible = true;
            optionButtonGroup.Visible(true);
            doneEnemyButton.Visible = false;
            browseEnemiesButton.Visible = true;

            if (enemyList.Count < enemyLimit) {
                newEnemyButton.Visible = true;
            }
        }
        #endregion

        #region Level Save Methods

        /// <summary>
        /// Moves the data from the editor into it's corresponding data model classes for JSON serialisation
        /// </summary>
        private static void SaveLevel() {
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    switch (gridElements[i, j].Type) {
                        case PaintSelection.WALL:
                            currentEditLevel.Data[i, j] = 0;
                            break;
                        case PaintSelection.PATH:
                            currentEditLevel.Data[i, j] = 1;
                            break;
                        default:
                            currentEditLevel.Data[i, j] = -1;
                            break;
                    }
                }
            }
            currentEditLevel.Enemies = new List<Enemy>();
            foreach (EditorEnemy enemy in enemyList) {
                Enemy newEnemy = new Enemy();
                newEnemy.X = enemy.XData;
                newEnemy.Y = enemy.YData;
                newEnemy.PatrolPoints = new List<Enemy.PatrolPoint>();

                foreach (EditorEnemy.Node node in enemy.Nodes) {
                    Enemy.PatrolPoint patrolPoint = new Enemy.PatrolPoint();
                    patrolPoint.X = node.XData;
                    patrolPoint.Y = node.YData;
                    newEnemy.PatrolPoints.Add(patrolPoint);
                }
                currentEditLevel.Enemies.Add(newEnemy);
            }
            if (isNewLevel) {
                levels.Add(currentEditLevel);
                levelSelectbuttons.Clear();
                isNewLevel = false;
            }
            LevelManager.WriteLevelsToFile();
        }

        private static void SaveEditorLevel() {
            const string message = "Are you sure you want to save your changes?";
            MessageBoxScreen confirmSaveMessageBox = new MessageBoxScreen(message);
            confirmSaveMessageBox.Accepted += ConfirmSaveMessageBoxAccepted;
            screenReference.ScreenManager.AddScreen(confirmSaveMessageBox);
        }

        private static void ConfirmSaveMessageBoxAccepted(object sender, EventArgs e) {
            SaveLevel();
        }
        #endregion

        #region Input Methods

        /// <summary>
        /// Checks for any input from the mouse - Mainly used to register clicks on buttons and the grid
        /// </summary>
        
        private static void EscapeCheck() {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                // QuitEditor();
                LoadingScreen.Load(screenReference.ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
            }
        }

        private static void QuitEditor() {
            const string message = "Are you sure you want to quit the editor?";
            MessageBoxScreen quitEditorMessageBox = new MessageBoxScreen(message);
            quitEditorMessageBox.Accepted += QuitEditorMessageBoxAccepted;
            screenReference.ScreenManager.AddScreen(quitEditorMessageBox);
        }

        private static void QuitEditorMessageBoxAccepted(object sender, EventArgs e) {
            UnloadLevelGrid();
            LoadingScreen.Load(screenReference.ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }

        private static void MouseInputCheck() {
            if (mouseState.LeftButton == ButtonState.Pressed) {

                if (previousMouseState.LeftButton == ButtonState.Released) {
                    foreach (UIButton btn in editorButtons) {
                        btn.HitTest(mouseState.Position);
                    }

                    if (isLevelSelectLoaded) {
                        foreach (UIButton btn in levelSelectbuttons) {
                            btn.HitTest(mouseState.Position);
                        }
                    }
                }

                if (isEnemySelectLoaded) {
                    foreach (UIButton btn in enemySelectButtons) {
                        btn.HitTest(mouseState.Position);
                    }
                }

                if (gridBounds.Contains(mouseState.Position)) {
                    int xStart, yStart;
                    xStart = MathHelper.Clamp(((mouseState.Position.X) / (elementSize + elementOffset) - 1), 0, gridSize - 1);
                    yStart = MathHelper.Clamp(((mouseState.Position.Y) / (elementSize + elementOffset) - 1), 0, gridSize - 1);

                    for (int y = yStart; y <= MathHelper.Clamp(y + 2, yStart, gridSize - 1); y++) {
                        for (int x = xStart; x <= MathHelper.Clamp(x + 2, xStart, gridSize - 1); x++) {
                            if (gridElements[x, y].HitTest(mouseState.Position)) {
                                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) {
                                    if (isPlacingEnemy) {
                                        isPlacingEnemy = false;
                                        EditorGridElement element = gridElements[x, y];
                                        // EditorEnemy enemy = new EditorEnemy(element.Position.X, element.Position.Y, element.Size, enemyTexture, nodeTexture);
                                        EditorEnemy enemy = new EditorEnemy(position, elementSize, elementOffset, enemyTexture, nodeTexture, element.XData, element.YData);
                                        selectedEnemy = enemy;
                                        enemyList.Add(enemy);
                                        doneEnemyButton.Visible = true;
                                        isPlacingNode = true;
                                    } else if (isPlacingNode) {
                                        EditorGridElement element = gridElements[x, y];
                                        selectedEnemy.addNode(new Vector2((position.X + elementSize * element.XData) + elementOffset * element.XData, (position.Y + elementSize * element.YData) + elementOffset * element.YData), element.XData, element.YData);
                                    }
                                }
                                return;
                            }

                        }
                    }
                }

            }

        }
        #endregion

        #endregion

        #region Update and Draw
        /// <summary>
        /// Engine Update Method
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (isLoaded) {
                EscapeCheck();
                MouseInputCheck();
            }
        }

        /// <summary>
        /// Engine Draw method. Sprite batch sent to all other drawing components in the Level Editor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch) {

            foreach (EditorGridElement element in gridElements) {
                element.Draw(spriteBatch);
            }

            if (selectedEnemy != null) {
                selectedEnemy.Draw(spriteBatch);
            }

            foreach (UIButton btn in editorButtons) {
                btn.Draw(spriteBatch);
            }

            if (isLevelSelectLoaded) {
                foreach (UIButton btn in levelSelectbuttons) {
                    btn.Draw(spriteBatch);
                }
            }

            if (isEnemySelectLoaded) {
                foreach (UIButton btn in enemySelectButtons) {
                    btn.Draw(spriteBatch);
                }
            }
        }
        #endregion

    }

}
