using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WpfAnimatedGif;

namespace VitoExtensions.SaddleUp.ToolWindows
{
    /// <summary>
    /// Interaction logic for SaddleUpSplashControlControl.
    /// </summary>
    public partial class SaddleUpSplashControl : UserControl
    {
        private readonly string _path = "pack://application:,,,/VitoExtensions.SaddleUp;component/Resources";

        private readonly string[] _gifs =
        [
            "horse_animated.gif",
            "horse_animated1.gif",
            "horse_animated2.gif",
            "horse_animated3.gif",
        ];

        private readonly string[] _phrases =
        [
            "Tabs? Gone.",
            "One tab to rule them all.",
            "All clear, captain.",
            "Your focus has arrived.",
            "Back to basics.",
            "They didn’t survive the ride.",
            "No tabs were harmed. Except all of them.",
            "It’s just you and the code now.",
            "Everyone else left. Drama much?",
            "You asked for silence. I delivered.",
            "The blade has fallen.",
            "Only the worthy remain.",
            "Clarity begins with closure.",
            "The code stays. The rest bows out.",
            "This IDE is now a dojo.",
            "Tabs fell like unsaved dreams.",
            "Cleared the stage. Spotlight’s yours.",
            "The noise left. You stayed.",
            "Focus, forged in flame.",
            "A clean slate, with attitude.",
            "They couldn’t keep up.",
            "Ride on. Tabs off.",
            "Tabs got bucked.",
            "This ride’s for pinned only.",
            "Mount up, code cowboy.",
            "A tab sneezed and vanished.",
            "The law of tab gravity reversed.",
            "Unicorns ate your distractions.",
            "We sacrificed them to the Build Gods.",
            "These tabs never existed. Trust me.",
            "NullReferenceException: TabNotFound",
            "IDE whispered 'shhh', and they obeyed.",
            "Tabs phased into another reality.",
            "Your tabs ascended.",
            "They left to start a jazz band."
        ];

        public SaddleUpSplashControl()
        {
            InitializeComponent();

            var random = new Random();

            var gifUri = new Uri($"{_path}/{_gifs[random.Next(_gifs.Length)]}");
            var phrase = _phrases[random.Next(_phrases.Length)];

            PhraseText.Text = phrase;

            var stream = Application.GetResourceStream(gifUri)?.Stream;
            if (stream != null)
            {
                ImageBehavior.SetAnimatedSource(GifImage, new BitmapImage(gifUri));
            }
        }

        public void RefreshQuote()
        {
            var random = new Random();
            var phrase = _phrases[random.Next(_phrases.Length)];
            PhraseText.Text = phrase;
        }
    }
}