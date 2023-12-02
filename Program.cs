using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Console_TestTask.Context;
using Console_TestTask.Context.Models;
using Console_TestTask.DataGenerators;

namespace Console_TestTask
{
    class Program
    {
        /// <summary>
        /// статическое поле для хранения экземпляра БД
        /// </summary>
        private static readonly ApplicationContext dbContext = ApplicationContext.Instance;

        /// <summary>
        /// статическое поле для хранения экземпляра генератора (IEmployeeGenerator)
        /// </summary>
        private static readonly IEmployeeGenerator employeeGenerator = new EmployeeGenerator();

        public static void Main(string[] args)
        {            
            ///выполнение проверок корректности команды и определение введенной команды
            
            if(args.Length == 0)
            {               
                Console.WriteLine("Argument empty entry error");
                return;
            }

            int.TryParse(args[0], out int command);

            switch (command)
            {
                case 1:
                    CreateTable();
                    break;
                case 2:
                    AddEmployee(args);
                    break;
                case 3:
                    IEnumerable<Employee> employees = SelectEmployees();
                    PrintEmployees(employees);
                    break;
                case 4:
                    DbEntryGenerationSaver();
                    break;
                case 5:
                    GetExecutionTime();
                    break;
                default: 
                    Console.WriteLine("Command not found");
                    break;
            };
        }

        /// <summary>
        /// статический метод имитирующий создание таблицы работников в БД
        /// </summary>
        private static void CreateTable()
        {
            /// если таблица существует в БД удялает ее. Новая таблица создается автоматически
            /// при использовании экземпляра БД
            dbContext.Employees.ExecuteDelete();
            Console.WriteLine("Table was created");
        }

        /// <summary>
        /// метод,добавляющий нового сотрудника в БД. Принимает строку параметров данных сотрудника
        /// </summary>
        /// <param name="args"></param>
        private static void AddEmployee(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Arguments length error");
                return;
            }

            /// проверка корректности данных
            if (!DateOnly.TryParse(args[2], out DateOnly date) ||
                !Enum.TryParse(args[3], out Gender gender))
            {
                Console.WriteLine("Entry error");
                return;
            }

            /// добавление сотрудника в коллекцию сотрудников
            dbContext.Employees.Add(new Employee(args[1], date, gender));

            /// сохранение изменений в БД
            try
            {
                dbContext.SaveChanges();
                Console.WriteLine("User successfully saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// метод вызывающий методы для генерации сотрудников и сохраняющий изменения в БД
        /// </summary>
        private static void DbEntryGenerationSaver()
        {
            DBEntryGenerate(1000000);
            DBEntryGenerate(100, "F");
            try
            {
                dbContext.SaveChanges();
                Console.WriteLine("Users successfully saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        /// <summary>
        /// метод генерирующий заданное количество записей БД и вставляет их в коллекцию сотрудников экземпляра БД
        /// </summary>
        /// <param name="autoEntryCount"></param>
        private static void DBEntryGenerate(int autoEntryCount)
        {
            for (int i = 0; i < autoEntryCount; i++)
            {
                dbContext.Employees.Add(employeeGenerator.Generate());
            }

        }

        /// <summary>
        /// перегрузка метода DBEntryGenerate, генерирующая заданнное количество записей 
        /// после генерации вставляет их в коллекцию сотрудников экземпляра БД
        /// </summary>
        /// <param name="autoEntryCount"></param>
        /// <param name="fioPattern"></param>
        private static void DBEntryGenerate(int autoEntryCount, string fioPattern)
        {
            for (int i = 0; i < autoEntryCount; i++)
            {
                dbContext.Employees.Add(new Employee(employeeGenerator.GenerateFIO(fioPattern),
                    employeeGenerator.GenerateDateOfBirth(),
                    employeeGenerator.GenerateGender()));
            }
        }

        /// <summary>
        /// метод, вычисляющий время выполнения запроса на выборку сотрудников. Выводит время выполнения в консоль
        /// </summary>
        private static void GetExecutionTime()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SelectEmployees(Gender.Male, "F");
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine($"Request execution time {elapsedTime}");
        }

        /// <summary>
        /// выполняет запрос на выборку уникальных записей (ФИО+дата) 
        /// </summary>
        /// <returns>возвращает результат выполнения запроса на выборку, коллекцию сотрудников</returns>
        private static IEnumerable<Employee> SelectEmployees()
        {
            return dbContext.Employees.AsEnumerable()
                .DistinctBy(x => x.FIO + x.DateOfBirth)
                .OrderBy(x => x.FIO);
        }        

        /// <summary>
        /// метод осуществляет выполнение запроса на выборку сотрудника с заданным полом и началом фамилии
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="fioPattern"></param>
        /// <returns>возвращает результат выполнения запроса на выборку, коллекцию сотрудников</returns>
        private static IEnumerable<Employee> SelectEmployees(Gender gender, string fioPattern)
        {
            /*return dbContext.Employees
                .Where(x => x.FIO.StartsWith(fioPattern) && x.Gender.Equals(gender.ToString()));*/
            return dbContext.Employees.Select(x => x)
                .Where(x => x.FIO.StartsWith(fioPattern) && x.Gender.Equals(gender.ToString())).AsNoTracking();
        }

        /// <summary>
        /// метод выводящий в консоль коллекцию сотрудников, переданную в качестве параметра
        /// </summary>
        /// <param name="uniqueEmployee"></param>
        private static void PrintEmployees(IEnumerable<Employee> uniqueEmployee)
        {
            foreach (Employee employee in uniqueEmployee)
            {
                Console.WriteLine(employee);
            }
        }
    }
}