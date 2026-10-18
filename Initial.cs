using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public string Name { get; private set; }
    public int Id { get; private set; }
    public List<double> Grades { get; private set; }

    public Student(string name, int id)
    {
        Name = name;
        Id = id;
        Grades = new List<double>();
    }

    public void AddGrade(double grade)
    {
        if (grade >= 0.0 && grade <= 100.0)
        {
            Grades.Add(grade);
        }
        else
        {
            Console.WriteLine("Ошибка: оценка должна быть от 0 до 100!");
        }
    }

    public double GetAverage()
    {
        if (Grades.Count == 0) return 0.0;
        return Grades.Average();
    }

    public void PrintInfo()
    {
        Console.WriteLine($"ID: {Id} | Имя: {Name} | Средний балл: {GetAverage():F2} | Оценок: {Grades.Count}");
    }
}

class GradeBook
{
    private List<Student> students = new List<Student>();
    private int nextId = 1001;

    public void AddStudent(string name)
    {
        students.Add(new Student(name, nextId));
        Console.WriteLine($"Студент \"{name}\" добавлен с ID {nextId}");
        nextId++;
    }

    public Student FindStudent(int id)
    {
        return students.FirstOrDefault(s => s.Id == id);
    }

    public void AddGradeToStudent(int id, double grade)
    {
        var s = FindStudent(id);
        if (s != null)
        {
            s.AddGrade(grade);
            Console.WriteLine($"Оценка {grade} добавлена студенту {s.Name}");
        }
        else
        {
            Console.WriteLine($"Студент с ID {id} не найден!");
        }
    }

    public void ShowAllStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст.");
            return;
        }
        Console.WriteLine("\n=== Список студентов ===");
        foreach (var s in students)
        {
            s.PrintInfo();
        }
        Console.WriteLine("========================");
    }

    public void ShowStudentDetails(int id)
    {
        var s = FindStudent(id);
        if (s == null)
        {
            Console.WriteLine("Студент не найден!");
            return;
        }
        Console.WriteLine("\n--- Детали студента ---");
        s.PrintInfo();
        if (s.Grades.Count > 0)
        {
            Console.WriteLine("Все оценки: " + string.Join(", ", s.Grades));
        }
        else
        {
            Console.WriteLine("Оценок пока нет.");
        }
        Console.WriteLine("-----------------------");
    }

    public void RemoveStudent(int id)
    {
        var s = FindStudent(id);
        if (s != null)
        {
            students.Remove(s);
            Console.WriteLine($"Студент с ID {id} удалён.");
        }
        else
        {
            Console.WriteLine($"Студент с ID {id} не найден.");
        }
    }

    public void ShowStatistics()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Нет данных для статистики.");
            return;
        }

        double totalAvg = 0.0;
        int countWithGrades = 0;
        double maxAvg = -1.0;
        double minAvg = 101.0;
        string topStudent = "";
        string bottomStudent = "";

        foreach (var s in students)
        {
            if (s.Grades.Count == 0) continue;
            double avg = s.GetAverage();
            totalAvg += avg;
            countWithGrades++;
            if (avg > maxAvg)
            {
                maxAvg = avg;
                topStudent = s.Name;
            }
            if (avg < minAvg)
            {
                minAvg = avg;
                bottomStudent = s.Name;
            }
        }

        Console.WriteLine("\n=== Статистика ===");
        Console.WriteLine($"Всего студентов: {students.Count}");
        if (countWithGrades > 0)
        {
            Console.WriteLine($"Средний балл по группе: {totalAvg / countWithGrades:F2}");
            Console.WriteLine($"Лучший студент: {topStudent} ({maxAvg:F2})");
            Console.WriteLine($"Студент с самым низким баллом: {bottomStudent} ({minAvg:F2})");
        }
        else
        {
            Console.WriteLine("У студентов пока нет оценок.");
        }
        Console.WriteLine("==================");
    }
}

class Program
{
    static void PrintMenu()
    {
        Console.WriteLine("\n========== Электронный журнал ==========");
        Console.WriteLine("1. Добавить студента");
        Console.WriteLine("2. Добавить оценку студенту");
        Console.WriteLine("3. Показать всех студентов");
        Console.WriteLine("4. Показать детали студента");
        Console.WriteLine("5. Удалить студента");
        Console.WriteLine("6. Показать статистику");
        Console.WriteLine("0. Выход");
        Console.WriteLine("========================================");
        Console.Write("Выберите действие: ");
    }

    static void Main()
    {
        var book = new GradeBook();

        Console.WriteLine("Добро пожаловать в простой электронный журнал студентов!");

        // Добавим несколько студентов для примера
        book.AddStudent("Иван Петров");
        book.AddStudent("Анна Сидорова");
        book.AddStudent("Алексей Козлов");
        book.AddGradeToStudent(1001, 85.5);
        book.AddGradeToStudent(1001, 90.0);
        book.AddGradeToStudent(1002, 78.0);
        book.AddGradeToStudent(1002, 82.5);
        book.AddGradeToStudent(1002, 91.0);
        book.AddGradeToStudent(1003, 95.0);

        while (true)
        {
            PrintMenu();
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Неверный ввод! Введите число.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Введите имя студента: ");
                    string name = Console.ReadLine()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        book.AddStudent(name);
                    }
                    else
                    {
                        Console.WriteLine("Имя не может быть пустым.");
                    }
                    break;

                case 2:
                    Console.Write("Введите ID студента: ");
                    if (int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.Write("Введите оценку (0-100): ");
                        if (double.TryParse(Console.ReadLine(), out double grade))
                        {
                            book.AddGradeToStudent(id2, grade);
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод оценки!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод ID!");
                    }
                    break;

                case 3:
                    book.ShowAllStudents();
                    break;

                case 4:
                    Console.Write("Введите ID студента: ");
                    if (int.TryParse(Console.ReadLine(), out int id4))
                    {
                        book.ShowStudentDetails(id4);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод!");
                    }
                    break;

                case 5:
                    Console.Write("Введите ID студента для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int id5))
                    {
                        book.RemoveStudent(id5);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод!");
                    }
                    break;

                case 6:
                    book.ShowStatistics();
                    break;

                case 0:
                    Console.WriteLine("До свидания!");
                    return;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}
