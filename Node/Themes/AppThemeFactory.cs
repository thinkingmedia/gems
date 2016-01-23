using System.Drawing;

namespace Node.Themes
{
    public static class AppThemeFactory
    {
        /// <summary>
        /// Creates a app theme object.
        /// </summary>
        public static iAppTheme Create(string pTitle, Icon pIcon)
        {
            return new AppTheme(pTitle, pIcon);
        }

        /// <summary>
        /// Creates a app theme object.
        /// </summary>
        public static iAppTheme Create(string pTitle, Bitmap pBitmap)
        {
            return new AppTheme(pTitle, Icon.FromHandle(pBitmap.GetHicon()));
        }
    }
}