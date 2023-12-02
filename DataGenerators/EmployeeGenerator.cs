using Console_TestTask.Context.Models;
using Faker;

namespace Console_TestTask.DataGenerators
{
    /// <summary>
    /// класс генератора работника, реализует интерфейс IEmployeeGenerator и IGenerator
    /// </summary>
    public class EmployeeGenerator : IEmployeeGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// реализация метода интерфейса IGenerator
        /// </summary>
        /// <returns>сгенерированный сотрудник</returns>
        public Employee Generate()
        {
            return new Employee(GenerateFIO(), GenerateDateOfBirth(), GenerateGender());
        }

        /// <summary>
        /// реализация метода интерфейса IEmployeeGenerator. Метод генерирует случайную дату рождения
        /// в диапазине от 1950г до (текущего года - 18 лет)
        /// </summary>
        /// <returns>сгенерированная дата рождения сотрудника</returns>
        public DateOnly GenerateDateOfBirth()
        {
            DateTime start = new DateTime(1950, 1, 1);
            DateTime majorityDate = new DateTime(DateTime.Now.Year - 18, 1, 1);
            int range = (majorityDate - start).Days;
            return DateOnly.FromDateTime(start.AddDays(random.Next(range)));
        }

        /// <summary>
        /// реализация метода интерфейса IEmployeeGenerator. Метод генерирует случайное ФИО с помощью Faker API
        /// </summary>
        /// <returns>сгенерированное ФИО сотрудника</returns>
        public string GenerateFIO()
        {
            return Name.FullName(NameFormats.StandardWithMiddle);
        }

        /// <summary>
        /// реализация метода интерфейса IEmployeeGenerator. Метод генерирует случайное ФИО с помощью Faker API 
        /// и осуществляет замену начала ФИО на переданный префикс
        /// </summary>
        /// <param name="fioPrefix"></param>
        /// <returns>сгенерированное ФИО сотрудника с заданным префиксом</returns>
        public string GenerateFIO(string fioPrefix)
        {
            return $"{fioPrefix}{GenerateFIO()[1..]}";
        }

        /// <summary>
        /// реализация метода интерфейса IEmployeeGenerator. Метод генерирует случайный пол с помощью Faker API
        /// </summary>
        /// <returns>сгенерированный пол сотрудника</returns>
        public Gender GenerateGender()
        {
            return Faker.Enum.Random<Gender>();
        }
    }
}
