using System;

namespace Antiplagiat
{
    public class Input
    {
        /// <summary>
        /// Данный метод будет первоначально спрашивать у пользователя, что нужно сделать
        /// </summary>
        /// <param name="choice"> В данную переменную мы запишем выбор пользователя </param>
        public void StartChoice(out int choice)
        {
            Console.WriteLine("Приветствуем Вас в программе антиплагиата!");
            Console.WriteLine("Что желаете сделать?");
            // заводим бесконечный цикл, чтобы в случае неправильного ввода от пользователя - повторить ввод данных
            while (true)
            {
                try
                {
                    Console.WriteLine("1 - Загрузить текст для проверки");
                    Console.WriteLine("2- Добавить текст в эталонные документы");
                    Console.WriteLine("3- Выйти");
                    int n = Convert.ToInt16(Console.ReadLine());
                    if (n != 1 && n != 2 && n != 3)
                    {
                        throw new ArgumentException();
                    }

                    choice = n;
                    break;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine(
                        "Введённое вами значение должно быть либо 1, либо 2, либо 3. Повторите, пожалуйста, ввод");
                }
                catch(Exception)
                {
                    Console.WriteLine("Введённое значение должно быть натуральным числом от 1 до 3. Повторите, пожалуйста, ввод");
                }
            }
        }


         /// <summary>
        /// Данный метод спрашивает у пользователя как он хочет ввести текст в антиплагиат
        /// </summary>
        /// <param name="choice">В данную переменную мы запишем выбор пользователя</param>
        /// <exception cref="ArgumentException"></exception>
        public void GetStringOrFile(out int choice)
        {
            Console.WriteLine("Выберите один из возможных вариантов загрузки текста");
            while (true)
            {
                try
                {
                    Console.WriteLine("1 - ввести текст в консоль");
                    Console.WriteLine("2 - ввести адрес файла на компьютере");
                    int n = Convert.ToInt16(Console.ReadLine());
                    if (n != 1 && n != 2)
                    {
                        throw new ArgumentException();
                    }

                    choice = n;
                    break;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Введённый выбор должен быть либо 1, либо 2. Повторите ввод");
                }
                catch (Exception)
                {
                    Console.WriteLine("Введённое значение должно быть натуральным числом от 1 до 3. Повторите, пожалуйста, ввод");
                }
            }
        }

        
        /// <summary>
        /// Данный метод спрашивает спрашивает у пользователя текст для антиплагиата 
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public string GetCurrentText(int choice)
        {
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Введите текст в консоль");
                    var text1 = Console.ReadLine();
                    return text1;
                case 2:
                    Console.WriteLine("Введите путь до файла на вашем компьютере");
                    var pathToFile = Console.ReadLine();
                    var text2 = File.ReadAllText(pathToFile , Encoding.GetEncoding(1251)); // данное исключение с null мы перехватим выше
                    return text2;
            }

            return null;
        }
    }
}
