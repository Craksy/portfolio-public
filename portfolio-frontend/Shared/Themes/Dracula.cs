using MudBlazor;
namespace Frontend.Shared.Themes 
{
    public class Dracula {
        public const string Background = "#282a36";
        public const string CurrentLine = "#44475a";
        public const string Foreground = "#f8f8f2";
        public const string Comment = "#6272a4";
        public const string Cyan = "#8be9fd";
        public const string Green = "#50fa7b";
        public const string Orange = "#ffb86c";
        public const string Pink = "#ff79c6";
        public const string Purple = "#bd93f9";
        public const string Red = "#ff5555";
        public const string Yellow = "#f1fa8c";

        public static string[] blacks = {
            "#1d2021",
            "#282828",
            "#504945",
            "#665c54",
            "#7c6f64",
            "#928374",
        };
        public static string[] whites = {
            "#fbf1c7",
            "#ebdbb2",
            "#d5c4a1",
            "#bdae93",
            "#a89984",
        };

        public static MudTheme draculaTheme = new MudTheme()
        {
            Palette = new Palette
            {
                Black = Background,
                White = whites[0],
                Primary = Purple,
                Secondary = Comment,
                Tertiary = Pink,
                Info = Comment,
                Success = Cyan,
                Warning = Yellow,
                Error = Red,
                Dark = blacks[2],
                TextPrimary = Foreground,
                TextSecondary = Foreground,
                TextDisabled = "#ebdbb260",
                ActionDefault = "#ebdbb289",
                ActionDisabled = "#ebdbb242",
                ActionDisabledBackground = "#ebdbb21e",
                Background = Background,
                BackgroundGrey = Background,
                Surface = Background,
                // DrawerBackground = background.dark,
                DrawerBackground = blacks[0],
                DrawerText = whites[3],
                DrawerIcon = whites[2],
                // AppbarBackground = green.dark,
                AppbarBackground = blacks[0],
                AppbarText = Foreground,
                LinesDefault = "#ebdbb2e1",
                LinesInputs = "#bdbdbdff",
                Divider = "#e0e0e0ff",
                DividerLight = "#ebdbb2cc",
                HoverOpacity = 0.26,
                GrayDefault = "#9E9E9E",
                GrayLight = "#BDBDBD",
                GrayLighter = "#E0E0E0",
                GrayDark = "#757575",
                GrayDarker = "#616161",
            },
            Shadows =
            {
                Elevation = new[]
                {
                    "none",
                    "0px 2px 1px -1px rgba(235,219,178,0.2),0px 1px 1px 0px rgba(235,219,178,0.14),0px 1px 3px 0px rgba(235,219,178,0.12)",
                    "0px 3px 1px -2px rgba(235,219,178,0.2),0px 2px 2px 0px rgba(235,219,178,0.14),0px 1px 5px 0px rgba(235,219,178,0.12)",
                    "0px 3px 3px -2px rgba(235,219,178,0.2),0px 3px 4px 0px rgba(235,219,178,0.14),0px 1px 8px 0px rgba(235,219,178,0.12)",
                    "0px 2px 4px -1px rgba(235,219,178,0.2),0px 4px 5px 0px rgba(235,219,178,0.14),0px 1px 10px 0px rgba(235,219,178,0.12)",
                    "0px 3px 5px -1px rgba(235,219,178,0.2),0px 5px 8px 0px rgba(235,219,178,0.14),0px 1px 14px 0px rgba(235,219,178,0.12)",
                    "0px 3px 5px -1px rgba(235,219,178,0.2),0px 6px 10px 0px rgba(235,219,178,0.14),0px 1px 18px 0px rgba(235,219,178,0.12)",
                    "0px 4px 5px -2px rgba(235,219,178,0.2),0px 7px 10px 1px rgba(235,219,178,0.14),0px 2px 16px 1px rgba(235,219,178,0.12)",
                    "0px 5px 5px -3px rgba(235,219,178,0.2),0px 8px 10px 1px rgba(235,219,178,0.14),0px 3px 14px 2px rgba(235,219,178,0.12)",
                    "0px 5px 6px -3px rgba(235,219,178,0.2),0px 9px 12px 1px rgba(235,219,178,0.14),0px 3px 16px 2px rgba(235,219,178,0.12)",
                    "0px 6px 6px -3px rgba(235,219,178,0.2),0px 10px 14px 1px rgba(235,219,178,0.14),0px 4px 18px 3px rgba(235,219,178,0.12)",
                    "0px 6px 7px -4px rgba(235,219,178,0.2),0px 11px 15px 1px rgba(235,219,178,0.14),0px 4px 20px 3px rgba(235,219,178,0.12)",
                    "0px 7px 8px -4px rgba(235,219,178,0.2),0px 12px 17px 2px rgba(235,219,178,0.14),0px 5px 22px 4px rgba(235,219,178,0.12)",
                    "0px 7px 8px -4px rgba(235,219,178,0.2),0px 13px 19px 2px rgba(235,219,178,0.14),0px 5px 24px 4px rgba(235,219,178,0.12)",
                    "0px 7px 9px -4px rgba(235,219,178,0.2),0px 14px 21px 2px rgba(235,219,178,0.14),0px 5px 26px 4px rgba(235,219,178,0.12)",
                    "0px 8px 9px -5px rgba(235,219,178,0.2),0px 15px 22px 2px rgba(235,219,178,0.14),0px 6px 28px 5px rgba(235,219,178,0.12)",
                    "0px 8px 10px -5px rgba(235,219,178,0.2),0px 16px 24px 2px rgba(235,219,178,0.14),0px 6px 30px 5px rgba(235,219,178,0.12)",
                    "0px 8px 11px -5px rgba(235,219,178,0.2),0px 17px 26px 2px rgba(235,219,178,0.14),0px 6px 32px 5px rgba(235,219,178,0.12)",
                    "0px 9px 11px -5px rgba(235,219,178,0.2),0px 18px 28px 2px rgba(235,219,178,0.14),0px 7px 34px 6px rgba(235,219,178,0.12)",
                    "0px 9px 12px -6px rgba(235,219,178,0.2),0px 19px 29px 2px rgba(235,219,178,0.14),0px 7px 36px 6px rgba(235,219,178,0.12)",
                    "0px 10px 13px -6px rgba(235,219,178,0.2),0px 20px 31px 3px rgba(235,219,178,0.14),0px 8px 38px 7px rgba(235,219,178,0.12)",
                    "0px 10px 13px -6px rgba(235,219,178,0.2),0px 21px 33px 3px rgba(235,219,178,0.14),0px 8px 40px 7px rgba(235,219,178,0.12)",
                    "0px 10px 14px -6px rgba(235,219,178,0.2),0px 22px 35px 3px rgba(235,219,178,0.14),0px 8px 42px 7px rgba(235,219,178,0.12)",
                    "0px 11px 14px -7px rgba(235,219,178,0.2),0px 23px 36px 3px rgba(235,219,178,0.14),0px 9px 44px 8px rgba(235,219,178,0.12)",
                    "0px 11px 15px -7px rgba(235,219,178,0.2),0px 24px 38px 3px rgba(235,219,178,0.14),0px 9px 46px 8px rgba(235,219,178,0.12)",
                    "0 5px 5px -3px rgba(235,219,178,.06), 0 8px 10px 1px rgba(235,219,178,.042), 0 3px 14px 2px rgba(235,219,178,.036)",

                }
            }
        };
    }
}
