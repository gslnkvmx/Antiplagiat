namespace Antiplagiat
{
    internal class Program
    {
        // здесь будет описана последовательность действий антиплагиата
        public static void Main(string[] args)
        {
           
            // инициализация объектов Input и Output
            var input = new Input();
            
            
            
            while (true)
            {
                int choice;
                input.StartChoice(out choice);
                
                // на данном шаге мы уверены что в choice лежит, либо 1, либо 2, либо 3
                switch (choice)
                {
                     case 1:
                         // далее спрашиваем у пользователя каким способом он хочет ввести текст в антиплагиат
                         string text;
                         while (true)
                         {
                             try
                             {
                                 input.GetStringOrFile(out choice);
                                 text = input.GetCurrentText(choice);
                                 break;
                             }
                             catch (IOException)
                             {
                                 Console.WriteLine("Произошла непредвиденная ошибка при загрузке текста, вероятно ошибка произошла при открытии файла. Повторите ,пожалуйста, ввод");
                             }
                             catch (Exception)
                             {
                                 Console.WriteLine("Произошла непредвиденная ошибка. Повторите ввод");
                             }
                         }
                         // на данном этапе у нас есть текст
                         //Console.WriteLine(text);
                         
                         // далее нужно обрабатывать данный текст
                         break;
                     case 2:
                         // in future
                         break;
                     case 3:
                         Console.WriteLine("Спасибо за работу в нашем антиплагиате!");
                         return;
                }
            }
        }
    }
}
