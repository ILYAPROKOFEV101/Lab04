
using System.Windows;
using System.Windows.Media;



using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab04
{
    public partial class MainWindow : Window
    {
        private SolidColorBrush currentBrushColor = Brushes.Black; // Текущий цвет кисти
        private double currentBrushSize = 5; // Текущий размер кисти
        private bool isDrawing = false; // Флаг для режима рисования
        private Point previousPoint; // Предыдущая точка для рисования линий
        private bool isDoubleClicked = false; // Флаг для отслеживания двойного щелчка

        private enum Mode
        {
            Draw,       // Режим рисования
            Erase,      // Режим стирания
            Delete      // Режим удаления объектов
        }

        private Mode currentMode = Mode.Draw; // Текущий режим по умолчанию

        public MainWindow()
        {
            InitializeComponent();
        }

        // Изменение цвета фона
        private void ChangeBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background = new SolidColorBrush(Colors.LightBlue);
            StatusText.Text = "Цвет фона изменён";
        }

        // Информация о разработчике
        private void AboutDeveloper_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработчик: Илья Прокофьев \nEmail: helloworld@gmail.com", "О программе");
            StatusText.Text = "Показана информация о разработчике";
        }

        // Закрытие приложения
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Обработка выбора цвета кисти
        private void BrushColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushColorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                currentBrushColor = new SolidColorBrush(((SolidColorBrush)selectedItem.Background).Color);
            }
        }

        // Обработка изменения размера кисти
        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentBrushSize = BrushSizeSlider.Value;
        }

        // Обработка выбора режима
        private void ModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                switch (radioButton.Content.ToString())
                {
                    case "Рисование":
                        currentMode = Mode.Draw;
                        break;
                    case "Стирание":
                        currentMode = Mode.Erase;
                        break;
                    case "Удаление":
                        currentMode = Mode.Delete;
                        break;
                }
            }
        }

        // Начало взаимодействия
        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) // Проверяем, что нажата именно левая кнопка мыши
            {
                Point mousePosition = e.GetPosition(DrawingCanvas);

                switch (currentMode)
                {
                    case Mode.Draw:
                        isDrawing = true; // Включаем режим рисования
                        previousPoint = mousePosition; // Сохраняем начальную точку
                        break;

                    case Mode.Erase:
                        var hitTestResult = VisualTreeHelper.HitTest(DrawingCanvas, mousePosition);
                        if (hitTestResult?.VisualHit is Shape shape)
                        {
                            shape.Stroke = new SolidColorBrush(Colors.White);
                            shape.Fill = new SolidColorBrush(Colors.White);
                        }
                        break;

                    case Mode.Delete:
                        hitTestResult = VisualTreeHelper.HitTest(DrawingCanvas, mousePosition);
                        if (hitTestResult?.VisualHit is Shape shapeToDelete)
                        {
                            DrawingCanvas.Children.Remove(shapeToDelete);
                        }
                        break;
                }
            }
        }
        // Рисование линии
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentMode == Mode.Draw && isDrawing) // Рисуем только если режим рисования активен и мышка зажата
            {
                Point currentPoint = e.GetPosition(DrawingCanvas);

                Line line = new Line
                {
                    X1 = previousPoint.X,
                    Y1 = previousPoint.Y,
                    X2 = currentPoint.X,
                    Y2 = currentPoint.Y,
                    Stroke = currentBrushColor,
                    StrokeThickness = currentBrushSize
                };

                DrawingCanvas.Children.Add(line);

                previousPoint = currentPoint; 
            }
        }
        // Окончание взаимодействия
        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        // Обработка наведения мыши на элементы управления
        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element && element.ToolTip is string tooltip)
            {
                StatusText.Text = tooltip;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element && element.ToolTip is string tooltip)
            {
                StatusText.Text = tooltip;
            }
        }
    }
}