using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using TraceabilityWebApi.Controllers;
using TraceabilityWebApi.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace Waga_Mobile
{
    class WagePageViewModel : INotifyPropertyChanged
    {

        public INavigation Navigation { get; set; }

        public WagePageViewModel(INavigation _navigation)
        {

        }

        public Command GetWage
        {
            get
            {
                return new Command(async() => 
                {
                    IsBusy = true;
                    using (var client = new HttpClient())
                    {
                        var url = "http://192.168.2.12:8081/api/Wage/GetWages";
                        var result = await client.GetStringAsync(url);

                        var WageList = JsonConvert.DeserializeObject<List<WageViewModel>>(result);

                        var wagaObj = WageList.First();

                        string wageReadString = removeSpace(wagaObj.wagaString);
                        
                        jednostka = wageReadString[wageReadString.Length - 2].ToString();
                        wagaString = wageReadString.Substring(0, wageReadString.Length - 2);


                        Wages = new ObservableCollection<WageViewModel>(WageList);

                        /*wagaString = WageList.F*/

                        IsBusy = false;
                    }
                });
            }
        }

        static string removeSpace(string str)
        {
            str = str.Replace(" ", "");
            return str;
        }


        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); }
        }


        string _jednostka;
        public string jednostka
        {
            get { return _jednostka; }
            set { _jednostka = value; OnPropertyChanged(); }
        }

        string _wagaString;
        public string wagaString
        {
            get { return _wagaString; }
            set { _wagaString = value; OnPropertyChanged(); }
        }



        ObservableCollection<WageViewModel> _wages;
        public ObservableCollection<WageViewModel> Wages
        {
            get
            {
                return _wages;
            }
            set
            {
                _wages = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
