using Console_TestTask.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace Console_TestTask.Context
{
    /// <summary>
    /// класс контекта БД
    /// </summary>
    internal class ApplicationContext : DbContext
    {
        /// <summary>
        /// статическое приватное поле экземпляра БД
        /// </summary>
        static ApplicationContext instance;

        /// <summary>
        /// публичное статическое свойство экземпляра БД. Реализация паттерна Singletone.
        /// Гарантируется создание только одного экземпляра БД
        /// </summary>
        public static ApplicationContext Instance => instance ??= new ApplicationContext();

        /// <summary>
        /// свойство типа DbSet<Employee> -
        /// коллекция сущностей из БД типа Employee (содержит работников из БД)
        /// </summary>
        public DbSet<Employee> Employees {get;set;}

        /// <summary>
        /// конструктор, создает БД если она еще не была создана
        /// </summary>
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// переопределение метода OnConfiguring, создает строку подключения к БД
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source = {System.AppContext.BaseDirectory}/employeeDirectory.db;");
        }

    }
}
