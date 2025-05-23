using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace TestMiniProj_1
{
    internal class Config
    {
        //public static string BASE_DIR { get; } = "C:/GDC_Unitec/6426 DataStrucNAlgrm/mySolution/TestMiniProj_1/";
        public static string BASE_DIR { get; } = Environment.CurrentDirectory + "/../../";
        public static string FONT_PATH { get; } = BASE_DIR + "resources/fonts/";
        public static string IMG_PATH { get; } = BASE_DIR + "resources/images/";
        public static Font DEAFULT_FONT { get; } = new Font(FONT_PATH + "arial.ttf");
        public static uint WIN_WIDTH { get; } = 800;
        public static uint WIN_HEIGHT { get; } = 600;
        public static string WIN_TITLE { get; } = "MiniProject of DSnA 2025";
        public static string STUDENT_INFO { get; } = "Chengfeng Li 1602741";
        public static int DISK_NUM { get; set; } = 2;
    }
}
