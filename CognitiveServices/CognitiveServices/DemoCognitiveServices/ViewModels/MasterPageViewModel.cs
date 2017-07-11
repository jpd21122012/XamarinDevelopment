using DemoCognitiveServices.Models;
using DemoCognitiveServices.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCognitiveServices.ViewModels
{
    public class MasterPageViewModel : ViewModelBase
    {
        private List<MasterPageItem> _items;

        public List<MasterPageItem> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(); }
        }

        public MasterPageViewModel()
        {
            Items = new List<MasterPageItem>();
            Load();
        }

        private void Load()
        {
            Items.Add(new MasterPageItem
            {
                Title = "Computer Vision Api",
                Icon = "ComputerIcon.png",
                Target = typeof(ComputerVisionApiPage)
            });
            Items.Add(new MasterPageItem
            {
                Title = "Text Recognition",
                Icon = "ComputerIcon.png",
                Target = typeof(TextRecognitionApiPage)
            });
            Items.Add(new MasterPageItem
            {
                Title = "Face Api",
                Icon = "FaceIcon.png",
                Target = typeof(FaceApiPage)
            });
            Items.Add(new MasterPageItem
            {
                Title = "Emotion Api",
                Icon = "EmotionIcon.png",
                Target = typeof(EmotionApiPage)
            });
           
            Items.Add(new MasterPageItem
            {
                Title = "About",
                Icon = "AboutIcon.png",
                Target = typeof(AboutPage)
            });
        }
    }
}
