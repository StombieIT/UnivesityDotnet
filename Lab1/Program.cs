using System.Reflection;

const char DEL = '-';
const int DEL_LENGTH = 30;
Dictionary<ConsoleKey, Type> TYPE_BY_KEY = new()
{
    { ConsoleKey.D1, typeof(uint) },
    { ConsoleKey.D2, typeof(int) },
    { ConsoleKey.D3, typeof(long) },
    { ConsoleKey.D4, typeof(float) },
    { ConsoleKey.D5, typeof(double) },
    { ConsoleKey.D6, typeof(char) },
    { ConsoleKey.D7, typeof(string) },
    // TODO: после переезда на 10 версию дотнета расскомментить 
    //{ ConsoleKey.D8, typeof(record) },
    { ConsoleKey.D9, typeof(Tuple<int, string>) },
};

Dictionary<ConsoleKey, ConsoleColor?> COLOR_BY_KEY = new()
{
    { ConsoleKey.D1, ConsoleColor.Magenta },
    { ConsoleKey.D2, ConsoleColor.White },
    { ConsoleKey.D3, ConsoleColor.Yellow },
    { ConsoleKey.D4, ConsoleColor.Cyan },
    { ConsoleKey.D5, ConsoleColor.Green },
    { ConsoleKey.D6, ConsoleColor.Black },
    { ConsoleKey.D7, ConsoleColor.Blue },
    { ConsoleKey.D8, ConsoleColor.DarkBlue },
    { ConsoleKey.D9, ConsoleColor.Gray },
};

char ConvertBoolToChar(bool value)
{
    return value ? '+' : '-';
}

void ChooseTypeFromList()
{

    while (true)
    {
        Console.WriteLine(
            "\nИнформация по типам\n" +
            new string(
                DEL,
                DEL_LENGTH
            ) + '\n' +
            "\t1 - uint\n" +
            "\t2 - int\n" +
            "\t3 - long\n" +
            "\t4 - float\n" +
            "\t5 - double\n" +
            "\t6 - char\n" +
            "\t7 - string\n" +
            "\t8 - record\n" +
            "\t9 - Type<int, string>\n" +
            "\t0 - Выход в главное меню\n"
        );

        ConsoleKey key = Console.ReadKey().Key;

        if (key == ConsoleKey.D0)
        {
            return;
        }

        var type = TYPE_BY_KEY.GetValueOrDefault(key);

        if (type == null)
        {
            continue;
        }

        PropertyInfo[] properties = type.GetProperties();
        FieldInfo[] fields = type.GetFields();
        MethodInfo[] methods = type.GetMethods();

        int methodsCount = methods.Length;
        int propertiesCount = properties.Length;
        int fieldsCount = fields.Length;

        Console.WriteLine(
            "\nИнформация по типу: " + type.Name + '\n' +
            "\tЗначимый тип: " + ConvertBoolToChar(type.IsValueType) + '\n' +
            "\tПространство имён: " + type.Namespace + '\n' +
            "\tСборка: " + type.Assembly.GetName() + '\n' +
            "\tОбщее число элементов: " + (methodsCount + propertiesCount + fieldsCount) + '\n' +
            "\tЧисло методов: " + methodsCount + '\n' +
            "\tЧисло свойств: " + propertiesCount + '\n' +
            "\tЧисло полей: " + fieldsCount + '\n' +
            "\tСписок полей: " + (fields.Length == 0 ? "-" : fields.Select(field => field.Name).Aggregate((n1, n2) => n1 + ", " + n2)) + '\n' +
            "\tСписок свойств: " + (properties.Length == 0 ? "-" : properties.Select(property => property.Name).Aggregate((n1, n2) => n1 + ", " + n2)) + '\n'
            );

        while (true)
        {
            Console.WriteLine(
                "\nНажмите ‘M’ для вывода дополнительной информации по методам;\n" +
                "Нажмите ‘0’ для выхода в главное меню\n"
                );

            ConsoleKey innerKey = Console.ReadKey().Key;

            if (innerKey == ConsoleKey.D0)
            {
                return;
            }

            if (innerKey == ConsoleKey.M)
            {
                break;
            }
        }

        Console.WriteLine(
            "Методы типа " + type.Name + '\n' +
            "Название\tЧисло перегрузок\tЧисло параметров"
            );

        foreach (var group in methods.GroupBy(method => method.Name))
        {
            var parametersCounts = group.Select(method => method.GetParameters().Length);
            string parametersCount = parametersCounts.Count() > 1
                ? $"{parametersCounts.Min()}..{parametersCounts.Max()}"
                : parametersCounts.FirstOrDefault().ToString();
            Console.WriteLine($"{group.Key}\t{group.Count()}\t{parametersCount}");
        }
    }
}

ConsoleColor? ChooseColor()
{
    while (true)
    {
        Console.WriteLine(
            "\nВыберите цвет:\n" +
            "1 - Magenta\n" +
            "2 - White\n" +
            "3 - Yellow\n" +
            "4 - Cyan\n" +
            "5 - Green\n" +
            "6 - Black\n" +
            "7 - Blue\n" +
            "8 - DarkBlue\n" +
            "9 - Gray\n" +
            "0 - Назад"
        );

        ConsoleKey key = Console.ReadKey().Key;

        if (key == ConsoleKey.D0)
        {
            return null;
        }

        ConsoleColor? color = COLOR_BY_KEY.GetValueOrDefault(key);

        if (color == null)
        {
            continue;
        }

        return color;
    }
}

void ChooseConsoleParameters()
{
    while (true)
    {
        Console.WriteLine(
            "\nВыберите параметр для изменения:\n" +
            "1 - Цвет текста\n" +
            "2 - Цвет заднего фона\n" +
            "0 - Выйти в главное меню"
        );

        ConsoleKey key = Console.ReadKey().Key;

        if (key == ConsoleKey.D0)
        {
            return;
        }

        if (key == ConsoleKey.D1 || key == ConsoleKey.D2)
        {
            ConsoleColor? color = ChooseColor();

            if (color == null)
            {
                continue;
            }

            if (key == ConsoleKey.D1)
            {
                Console.ForegroundColor = color ?? ConsoleColor.White;
            }

            if (key == ConsoleKey.D2)
            {
                Console.BackgroundColor = color ?? ConsoleColor.White;
            }

            Console.Clear();

            return;
        }
    }
}

// Интерфейс с наибольшим числом элементов
Type? GetInterfaceWithMaxMembers(List<Type> types)
{
    return types.Where(type => type.IsInterface)
        .OrderBy(type => type.GetFields().Length + type.GetMethods().Length + type.GetProperties().Length)
        .LastOrDefault();
}
// Тип, у которого конструктор имеет наибольшее число аргументов
Type? GetTypeWithManyArgsConstr(List<Type> types)
{
    return types.Where(type => !type.IsInterface && type.GetConstructors().Length != 0)
        .OrderBy(type => type.GetConstructors().Max(c => c.GetCustomAttributesData().Count))
        .LastOrDefault();
}
// Самое длинное название типа
Type? GetTypeWithLongestName(List<Type> types)
{
    return types.OrderBy(type => type.Name.Length)
        .LastOrDefault();
}

void PrintCommonTypeInformation()
{
    Assembly currentAssembly = Assembly.GetExecutingAssembly();

    List<Type> types = currentAssembly.GetTypes().ToList();

    Console.WriteLine(
        "\nОбщая информация по типам\n" +
        $"Подключённые сборки: {currentAssembly.GetReferencedAssemblies().Length}\n" +
        $"Всего типов по всем подключённым сборкам: {types.Count}\n" +
        $"Ссылочные типы (только классы): {types.Where(t => !t.IsValueType).Count()}\n" +
        $"Значимые типы: {types.Where(t => t.IsValueType).Count()}\n" +
        $"Интерфейс с наибольшим числом элементов: {GetInterfaceWithMaxMembers(types)?.Name ?? "-"}\n" +
        $"Тип, у которого конструктор имеет наибольшее число аргументов: {GetTypeWithManyArgsConstr(types)?.Name ?? "-"}\n" +
        $"Самое длинное название типа: {GetTypeWithLongestName(types)?.Name ?? "-"}"
        );
}

while (true)
{
    Console.WriteLine(
        "\nИнформация по типам:\n" +
        "1 - Общая информация по типам\n" +
        "2 - Выбрать тип из списка\n" +
        "3 - Параметры консоли\n" +
        "0 - Выход из программы"
        );

    ConsoleKey key = Console.ReadKey().Key;

    switch (key)
    {
        // 6 вариант
        case ConsoleKey.D1:
            PrintCommonTypeInformation();
            break;
        case ConsoleKey.D2:
            ChooseTypeFromList();
            break;
        case ConsoleKey.D3:
            ChooseConsoleParameters();
            break;
        case ConsoleKey.D0:
            return;
        default:
            break;

    }
}