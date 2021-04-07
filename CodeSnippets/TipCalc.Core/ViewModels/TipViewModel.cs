using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using RandomString;
using TipCalc.Core.Services;

//References:
//https://www.mvvmcross.com/documentation/tutorials/tipcalc/the-core-project
//https://github.com/MvvmCross/MvvmCross-Samples/tree/master/TipCalc/TipCalc.Core
//https://www.youtube.com/watch?v=8E000zu8UhQ   IAmTimCorey
//https://www.codeproject.com/Articles/5273075/MvvmCross-for-WPF-A-Basic-Primer
//https://github.com/MeshackMusundi/StaffStuff-MvvmCross/blob/master/StaffStuff.WPF/App.xaml
namespace TipCalc.Core.ViewModels
{
    public class TipViewModel : MvxViewModel
    {
        private readonly ICalculationService _calculationService;

        public TipViewModel(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            Passwords = new ObservableCollection<string>();
            SubTotal = 100;
            Generosity = 10;
            Recalcuate();

            //Simple init
            Passwords.Add("Alfa");
            Passwords.Add("Beta");
            Passwords.Add("Gamma");
            Passwords.Add("Delta");
        }

        private ObservableCollection<string> _passwords; //= new ObservableCollection<string>();
        public ObservableCollection<string> Passwords
        {
            get => _passwords;
            set
            {
                _passwords = value;
                RaisePropertyChanged(() => Passwords);
            }
        }

        MvxCommand _myAwesomeCommand;

        /* public IMvxCommand GetPasswordsCmd
        {
            get { return new MvxCommand(GetPasswords); }
        } */

        public IMvxAsyncCommand GetPasswordsCmd
        {
            get { return new MvxAsyncCommand(GetPasswords); }
        }

        //public IMvxAsyncCommand GetPasswordsCmd => new MvxAsyncCommand(GetPasswords);

        //Processed asynchronously
        private async Task GetPasswords()
        {
            Debug.WriteLine("Entering GetPasswords().");

            string randomPassword;
            Passwords.Clear();
            IRandomStringGenerator _randomStringGenerator = new RandomStringGenerator();

            for(int i = 0; i < 100; i++)
            {
                randomPassword = _randomStringGenerator.Next(AllowedCharacters.LowerCaseLetters |
                    AllowedCharacters.UpperCaseLetters |
                    AllowedCharacters.Digits
                    , 20, false);
                Passwords.Add($"{i}, {randomPassword}");
                await Task.Delay(50);
            }

            Debug.WriteLine("Exiting GetPasswords().");
        }

        private double _subTotal;
        public double SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                RaisePropertyChanged(() => SubTotal);

                Recalcuate();
            }
        }

        private int _generosity;
        public int Generosity
        {
            get => _generosity;
            set
            {
                _generosity = value;
                RaisePropertyChanged(() => Generosity);

                Recalcuate();
            }
        }

        private double _tip;
        public double Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                RaisePropertyChanged(() => Tip);
            }
        }

        private void Recalcuate()
        {
            Tip = _calculationService.TipAmount(SubTotal, Generosity);
        }
    }
}
