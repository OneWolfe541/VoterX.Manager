﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoterX.Core.Voters;

namespace VoterX.Manager.Views.Ballots
{
    /// <summary>
    /// Interaction logic for ProvisionalBallotPage.xaml
    /// </summary>
    public partial class ProvisionalBallotPage : Page
    {
        private NMVoter _voter;

        private bool _editStyle;

        public ProvisionalBallotPage(NMVoter voter, bool editStyle)
        {
            InitializeComponent();

            _voter = voter;

            _editStyle = editStyle;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ProvisionalBallotViewModel(_voter, _editStyle);
        }
    }
}
