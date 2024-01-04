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
                         // далее спрашиваем у пользователя каким способом он хочет ввести число
                         input.GetStringOrFile(out choice);
                         
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
