﻿using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Card_Creator
{

    public partial class Create_Card_Window : Window
    {
        CardType currentType;
        List<CardType> cardTypes;

        public Create_Card_Window()
        {
            InitializeComponent();

            UpdateSettings.UpdateDarkMode(this);

            ReadDatabase();

            if(cardTypes.Count > 0)
            {
                create_card_comboBox_type.SelectedIndex = 0;
            }
            else
            {
                edit_type_button.IsEnabled = false;
            }
        }

        private void Add_type_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Type_Window add_Type_Window = new Add_Type_Window(false, null);
            add_Type_Window.ShowDialog();
            ReadDatabase();
        }

        private void Edit_type_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Type_Window add_Type_Window = new Add_Type_Window(true, currentType);
            add_Type_Window.ShowDialog();
            ReadDatabase();
        }

        private void Generate_card_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Select_Image_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Create_card_comboBox_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentType = (CardType)create_card_comboBox_type.SelectedItem;

            if(create_card_comboBox_type.SelectedIndex >= 0)
            {
                edit_type_button.IsEnabled = true;
            }

            if(currentType != null)
            {
                Card_Preview.type.Content = "Type: " + currentType.Name;
                Card_Preview.borderColor.BorderBrush = (Brush)new BrushConverter().ConvertFromString(currentType.Cardcolor);
            }


        }

        void ReadDatabase()
        {
            using (CardContext context = new CardContext())
            {
                cardTypes = context.CardTypes.ToList();

                if(cardTypes.Count > 0)
                {
                    create_card_comboBox_type.ItemsSource = cardTypes;

                    create_card_comboBox_type.SelectedIndex = 0;

                    currentType = (CardType)create_card_comboBox_type.SelectedItem;

                    if (currentType != null)
                    {
                        create_card_comboBox_type.SelectedIndex = currentType.ID;
                    }
                }
                else
                {
                    create_card_comboBox_type.SelectedIndex = -1;
                    create_card_comboBox_type.ItemsSource = null;
                    edit_type_button.IsEnabled = false;
                }
            }
        }

        private void Name_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Card_Preview.name.Content = name_textBox.Text;
        }
    }
}
