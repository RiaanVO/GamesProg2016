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

        // TO-DO: Add ability to select what to paint on to the grid
        // Save the level - Add it as a level to the list
        // - Will need to check for viable path
        // - Must have start point and end point
        // Load level - Display button for each level present
        // Speed up grid element search [x]

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
        private static MouseState mouseState;

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
        private static Texture2D buttonTexture, buttonSelectedTexture;
        private static UIButton newButton, loadButton, saveButton;
        private static EditorButtonGroup optionButtonGroup;
        private static EditorOptionButton emptyButton, wallButton, pathButton;
        private static int editorButtonPadding = 5;
        private static bool isLevelSelectLoaded = false;

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

        public static bool Loaded {
            get { return isLoaded; }
        }
        #endregion

        #region Initialisation
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

        private static void InitialiseEditorButtons() {
            newButton = new UIButton("newButton", new Vector2(450, 20), Vector2.Zero, editorFont, "New Level", Color.White, buttonTexture);
            newButton.Visible = true;
            newButton.Padding = editorButtonPadding;
            newButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(newButton);

            loadButton = new UIButton("loadButton", new Vector2(550, 20), Vector2.Zero, editorFont, "Load Level", Color.White, buttonTexture);
            loadButton.Visible = true;
            loadButton.Padding = editorButtonPadding;
            loadButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(loadButton);

            saveButton = new UIButton("saveButton", new Vector2(650, 20), Vector2.Zero, editorFont, "Save Level", Color.White, buttonTexture);
            saveButton.Visible = true;
            saveButton.Padding = editorButtonPadding;
            saveButton.Clicked += new UIButton.ClickHandler(EditorButtonOnClick);
            editorButtons.Add(saveButton);

            optionButtonGroup = new EditorButtonGroup();

            emptyButton = new EditorOptionButton("emptyOptionButton", new Vector2(120, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Clear", Color.White, buttonTexture, buttonSelectedTexture, 50);
            emptyButton.Visible = true;
            emptyButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            emptyButton.Selected = true;
            optionButtonGroup.AddButton(emptyButton);
            editorButtons.Add(emptyButton);

            wallButton = new EditorOptionButton("wallOptionButton", new Vector2(180, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Wall", Color.White, buttonTexture, buttonSelectedTexture, 50);
            wallButton.Visible = true;
            wallButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            optionButtonGroup.AddButton(wallButton);
            editorButtons.Add(wallButton);

            pathButton = new EditorOptionButton("pathOptionButton", new Vector2(240, device.Viewport.Height - 40), Vector2.Zero, editorFont, "Path", Color.White, buttonTexture, buttonSelectedTexture, 50);
            pathButton.Visible = true;
            pathButton.Clicked += new UIButton.ClickHandler(OptionButtonOnClick);
            optionButtonGroup.AddButton(pathButton);
            editorButtons.Add(pathButton);
        }
        #endregion

        #region OnClick Handlers
        private static void OptionButtonOnClick(Object sender, EventArgs e) {
            EditorOptionButton button = (EditorOptionButton)sender;
            if (button.Selected == false) {
                switch (button.ID) {
                    case "emptyOptionButton":
                        selectedPaint = PaintSelection.EMPTY;
                        break;
                    case "wallOptionButton":
                        selectedPaint = PaintSelection.WALL;
                        break;
                    case "pathOptionButton":
                        selectedPaint = PaintSelection.PATH;
                        break;
                }
                optionButtonGroup.ToggleButton(button);
            }
        }

        private static void EditorButtonOnClick(Object sender, EventArgs e) {
            if (sender == newButton) {
                UnloadLevelGrid();
                GenerateNewGrid();
                return;
            }

            if (sender == saveButton) {
                SaveLevel();
                return;
            }

            if (sender == loadButton) {
                LoadLevelSelectionGrid();
                return;
            }
        }

        private static void LevelSelectOnClick(Object sender, EventArgs e) {
            UIButton button = (UIButton)sender;

            loadButton.Visible = true;
            saveButton.Visible = true;
            newButton.Visible = true;

            isLevelSelectLoaded = false;

            UnloadLevelGrid();
            LoadLevelGrid(int.Parse(button.ID));
        }
        #endregion

        #region Main Editor Methods
        private static void LoadLevelSelectionGrid() {
            loadButton.Visible = false;
            saveButton.Visible = false;
            newButton.Visible = false;

            int numLevels = levels.Count;
            double numColumns = Math.Ceiling(numLevels / 4.00);



            if (levelSelectbuttons.Count == 0) {
                int counter = 1;
                for (int y = 0; y <= numColumns - 1; y++) {
                    for (int x = 0; x < 4; x++) {
                        UIButton newButton = new UIButton(counter.ToString(), new Vector2(500 + 60 * x, 50 + 30 * y), Vector2.Zero, editorFont, "Level " + counter.ToString() , Color.White, buttonTexture);
                        newButton.Visible = true;
                        newButton.Clicked += new UIButton.ClickHandler(LevelSelectOnClick);
                        levelSelectbuttons.Add(newButton);
                        if(counter++ == numLevels) {
                            break;
                        }
                    }
                }
            }
            isLevelSelectLoaded = true;
        }

        private static void InitialiseGrid() {
            gridBounds = new Rectangle((int)position.X, (int)position.Y, elementSize * gridSize + elementOffset * gridSize, elementSize * gridSize + elementOffset * gridSize);
            GenerateNewGrid();
        }

        private static void GenerateNewGrid() {
            isNewLevel = true;
            currentEditLevel = new Level();
            currentEditLevel.Id = levels[levels.Count - 1].Id + 1;
            currentEditLevel.Data = new int[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    gridElements[i, j] = CreateNewElement(defaultTexture, PaintSelection.EMPTY, i, j);
                }
            }
        }

        private static void LoadLevelGrid(int id) {
            isNewLevel = false;
            currentEditLevel = levels[id - 1];

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    switch(currentEditLevel.Data[i, j]) {
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
        }

        private static EditorGridElement CreateNewElement(Texture2D texture, PaintSelection selection, int i, int j) {
            EditorGridElement element = new EditorGridElement(texture, (position.X + elementSize * i) + elementOffset * i, (position.Y + elementSize * j) + elementOffset * j, elementSize);
            element.Type = selection;
            element.Obstacle = EditorGridElement.ElementObstacle.NONE;
            return element;
        }

        private static void UnloadLevelGrid() {
            gridElements = new EditorGridElement[gridSize, gridSize];
        }

        private static void SaveLevel() {
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    switch(gridElements[i, j].Type) {
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
            if (isNewLevel) {
                levels.Add(currentEditLevel);
                levelSelectbuttons.Clear();
                isNewLevel = false;
            }
            LevelManager.WriteLevelsToFile();
        }

        private static void MouseInputCheck() {
            if (mouseState.LeftButton == ButtonState.Pressed) {
                if (gridBounds.Contains(mouseState.Position)) {
                    int xStart, yStart;
                    xStart = MathHelper.Clamp(((mouseState.Position.X) / (elementSize + elementOffset) - 1), 0, gridSize - 1);
                    yStart = MathHelper.Clamp(((mouseState.Position.Y) / (elementSize + elementOffset) - 1), 0, gridSize - 1);

                    for (int y = yStart; y <= MathHelper.Clamp(y + 2, yStart, gridSize - 1); y++) {
                        for (int x = xStart; x <= MathHelper.Clamp(x + 2, xStart, gridSize - 1); x++) {
                            if (gridElements[x, y].HitTest(mouseState.Position)) {
                                return;
                            }

                        }
                    }
                }

                foreach (UIButton btn in editorButtons) {
                    btn.HitTest(mouseState.Position);
                }
                
                if (isLevelSelectLoaded) {
                    foreach (UIButton btn in levelSelectbuttons) {
                        btn.HitTest(mouseState.Position);
                    }
                }
            }

        }
        #endregion

        #region Update and Draw
        public static void Update(GameTime gameTime) {
            mouseState = Mouse.GetState();
            if (isLoaded) {
                MouseInputCheck();
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            foreach (EditorGridElement element in gridElements) {
                element.Draw(spriteBatch);
            }

            foreach (UIButton btn in editorButtons) {
                btn.Draw(spriteBatch);
            }

            if (isLevelSelectLoaded) {
                foreach (UIButton btn in levelSelectbuttons) {
                    btn.Draw(spriteBatch);
                }
            }
        }
        #endregion

    }

}
