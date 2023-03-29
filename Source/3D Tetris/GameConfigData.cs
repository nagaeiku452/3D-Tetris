using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Globalization;
using _3D_Tetris.Drawing;

namespace _3D_Tetris
{
    public static class GameConfigData
    {
        private static int gameLoopInterval = 15;// in millisecond
        private static int renderInterval = 15;// in millisecond
        private static int worldLengthX = 4;
        private static int worldLengthY = 4;
        private static int worldLengthZ = 15;
        private static int worldTetrisUnitBoxHeight = 38;
        private static int worldTetrisUnitZSurfaceWidth = 62;
        private static int worldTetrisUnitZSurfaceHeight = 31;
        private static int tetrisModelUnitZSurfaceHeight = 18;
        private static int tetrisModelUnitZSurfaceWidth = 36;
        private static int tetrisModelUnitBoxHeight = 22;
        private static int heldTetrisUnitZSurfaceHeight = 23;
        private static int heldTetrisUnitZSurfaceWidth = 46;
        private static int heldTetrisUnitBoxHeight = 28;
        private static float groundEdgeWidth = 15f;
        private static int initialLevel = 1;
        private static int clearedSlicesRequiredForLevelUp = 10;

        public static string TetrisUpRightMove { get; private set; } = "Up";
        public static string TetrisDownLeftMove { get; private set; } = "Down";
        public static string TetrisUpLeftMove { get; private set; } = "Left";
        public static string TetrisDownRightMove { get; private set; } = "Right";




        public static int GameLoopInterval
        {
            get => gameLoopInterval;
            private set
            {
                if (value > 0)
                {
                    gameLoopInterval = value;
                }
            }
        }
        public static int RenderInterval
        {
            get => renderInterval; private set
            {
                if (value > 0)
                {
                    renderInterval = value;
                }
            }
        }

        public static int WorldLengthX
        {
            get => worldLengthX;
            private set
            {
                if (value > 0)
                {
                    worldLengthX = value;
                }
            }
        }
        public static int WorldLengthY
        {
            get => worldLengthY;
            private set
            {
                if (value > 0)
                {
                    worldLengthY = value;
                }
            }
        }
        public static int WorldLengthZ
        {
            get => worldLengthZ;
            private set
            {
                if (value > 0)
                {
                    worldLengthZ = value;
                }
            }
        }

        //public const int backgroundcolorR = 0xF7;
        //public const int backgroundcolorG = 0xF7;
        //public const int backgroundcolorB = 0xF7;
        public static byte BackgroundColorR { get; private set; } = 0x22;
        public static byte BackgroundColorG { get; private set; } = 0x22;
        public static byte BackgroundColorB { get; private set; } = 0x22;

        public static Color BackgroundColor => Color.FromRgb(BackgroundColorR, BackgroundColorG, BackgroundColorB);

        public static int WorldTetrisUnitZSurfaceHeight
        {
            get => worldTetrisUnitZSurfaceHeight;
            private set
            {
                if (value > 0)
                {
                    worldTetrisUnitZSurfaceHeight = value;
                }
            }
        }
        public static int WorldTetrisUnitZSurfaceWidth
        {
            get => worldTetrisUnitZSurfaceWidth;
            private set
            {
                if (value > 0)
                {
                    worldTetrisUnitZSurfaceWidth = value;
                }
            }
        }
        public static int WorldTetrisUnitBoxHeight
        {
            get => worldTetrisUnitBoxHeight;
            private set
            {
                if (value > 0)
                {
                    worldTetrisUnitBoxHeight = value;
                }
            }
        }

        public static int TetrisModelUnitZSurfaceHeight
        {
            get => tetrisModelUnitZSurfaceHeight;
            private set
            {
                if (value > 0)
                {
                    tetrisModelUnitZSurfaceHeight = value;
                }
            }
        }
        public static int TetrisModelUnitZSurfaceWidth
        {
            get => tetrisModelUnitZSurfaceWidth;
            private set
            {
                if (value > 0)
                {
                    tetrisModelUnitZSurfaceWidth = value;
                }
            }
        }
        public static int TetrisModelUnitBoxHeight
        {
            get => tetrisModelUnitBoxHeight;
            private set
            {
                if (value > 0)
                {
                    tetrisModelUnitBoxHeight = value;
                }
            }
        }

        public static int HeldTetrisUnitZSurfaceHeight
        {
            get => heldTetrisUnitZSurfaceHeight;
            private set
            {
                if (value > 0)
                {
                    heldTetrisUnitZSurfaceHeight = value;
                }
            }
        }
        public static int HeldTetrisUnitZSurfaceWidth
        {
            get => heldTetrisUnitZSurfaceWidth;
            private set
            {
                if (value > 0)
                {
                    heldTetrisUnitZSurfaceWidth = value;
                }
            }
        }
        public static int HeldTetrisUnitBoxHeight
        {
            get => heldTetrisUnitBoxHeight;
            private set
            {
                if (value > 0)
                {
                    heldTetrisUnitBoxHeight = value;
                }
            }
        }

        public static byte XSurfaceBrightness { get; private set; } = 135;
        public static byte YSurfaceBrightness { get; private set; } = 195;
        public static byte ZSurfaceBrightness { get; private set; } = 255;

        public static int SideWallHeightOffset { get; private set; } = 10;

        public static float GroundEdgeWidth
        {
            get => groundEdgeWidth;
            private set
            {
                if (value > 0)
                {
                    groundEdgeWidth = value;
                }
            }
        }

        public static byte GroundEdgeColorR { get; private set; } = 0xEE;
        public static byte GroundEdgeColorG { get; private set; } = 0xEE;
        public static byte GroundEdgeColorB { get; private set; } = 0xEE;

        public static int CurrentTetrisInitPosX { get; private set; } = 1;
        public static int CurrentTetrisInitPosY { get; private set; } = 1;

        public static int TetrisWorldGravityScalar { get; private set; } = 1000000;

        public static int TetrisFallInterval { get; private set; } = 107;//in intervel count
        public static int TetrisSoftDropInterval { get; private set; } = 3;//in intervel count

        public static int InitialLevel
        {
            get => initialLevel;
            private set
            {
                if (value > 0)
                {
                    initialLevel = value;
                }
            }
        }
        public static int ClearedSlicesRequiredForLevelUp
        {
            get => clearedSlicesRequiredForLevelUp;
            private set
            {
                if (value > 0)
                {
                    clearedSlicesRequiredForLevelUp = value;
                }
            }
        }


        public static byte CurrentTetrisShadowColorA { get; private set; } = 127;
        public static byte CurrentTetrisShadowColorR { get; private set; } = 0;
        public static byte CurrentTetrisShadowColorG { get; private set; } = 0;
        public static byte CurrentTetrisShadowColorB { get; private set; } = 0;



        private static string ConfigPath { get; } = "./config";

        static GameConfigData()
        {
            if (!File.Exists(ConfigPath))
            {
                return;
            }

            string[] data = File.ReadAllLines(ConfigPath, Encoding.Unicode);

            foreach (string s in data)
            {
                if (!s.StartsWith("//", StringComparison.Ordinal))
                {
                    int position = s.IndexOf('=', 0);
                    if (position != -1)
                    {
                        string argName = s[..position];
                        string argValue = s[(position + 1)..];
                        ConvertData(argName, argValue);
                    }
                }
            }
        }

        private static void ConvertData(string argName, string argValue)
        {
            try
            {
                Type configDataType = typeof(GameConfigData);
                PropertyInfo property = configDataType.GetProperty(argName);
                property?.SetValue(null, Convert.ChangeType(argValue, property.PropertyType, CultureInfo.InvariantCulture));
            }
            catch
            {
                //any exception occurs will result in no config data is read.
            }
        }
    }
}
