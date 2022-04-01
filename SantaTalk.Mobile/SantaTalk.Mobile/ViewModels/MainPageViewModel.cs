using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;
using SantaTalk.Mobile.Views;

namespace SantaTalk.Mobile.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SendLetterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(KidsName, LetterText));
            });

            ScanLetterCommand = new Command<bool>(async (useCamera) =>
            {
                await ScanLetterForSanta(useCamera);
            });
        }

        string kidsName;
        public string KidsName
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText = "Dear Santa...";
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        public ICommand SendLetterCommand { get; }
        public ICommand ScanLetterCommand { get; }

        private async Task ScanLetterForSanta(bool useCamera)
        {
            var photoService = new PhotoService();

            var photo = await photoService.GetPhoto(useCamera ? ImageMode.Camera : ImageMode.Gallery);
            var path = await CopyImage(photo);
            var stream = ReadFile(path);

            var scanService = new LetterScanService();
            var scannedLetter = await scanService.ScanLetterForSanta(stream);

            LetterText = scannedLetter;
        }

        private async Task<string> CopyImage(FileResult photo)
        {
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            return newFile;
        }

        private Stream ReadFile(string path)
        {
            return new FileStream(path, FileMode.Open);
        }
    }
}