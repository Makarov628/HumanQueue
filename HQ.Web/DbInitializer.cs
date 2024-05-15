using System.Net;
using System.Net.Mail;
using System.Text;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.UserAggregate;
using HQ.Domain.WindowAggregate;
using HQ.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(HQDbContext context)
    {
        context.Database.EnsureCreated();
        if (context.Queues.Any())
        {
            return;   // DB has been seeded
        }

        //////////   ОЧЕРЕДЬ   ////////// 

        var queue = QueueAggregate.Create("Приемная комиссия 2023", Culture.Create("ru").Value);
        context.Queues.Add(queue);


        //////////   ПОЛЬЗОВАТЕЛИ   ////////// 

        var admin1 = UserAggregate.Create("Администратор", "1", login: "admin", password: "1qaz1qaz", email: null, isAdmin: true);
        var operator1 = UserAggregate.Create("Оператор", "1", login: "operator1", password: "1", email: null, isAdmin: false);
        var operator2 = UserAggregate.Create("Оператор", "2", login: "operator2", password: "2", email: null, isAdmin: false);
        var operator3 = UserAggregate.Create("Оператор", "3", login: "operator3", password: "3", email: null, isAdmin: false);
        var operator4 = UserAggregate.Create("Оператор", "4", login: "operator4", password: "4", email: null, isAdmin: false);
        var operator5 = UserAggregate.Create("Оператор", "5", login: "operator5", password: "5", email: null, isAdmin: false);
        var operator6 = UserAggregate.Create("Оператор", "6", login: "operator6", password: "6", email: null, isAdmin: false);
        var operator7 = UserAggregate.Create("Оператор", "7", login: "operator7", password: "7", email: null, isAdmin: false);
        var operator8 = UserAggregate.Create("Оператор", "8", login: "operator8", password: "8", email: null, isAdmin: false);

        context.Users.AddRange(
            admin1,
            operator1,
            operator2,
            operator3,
            operator4,
            operator5,
            operator6,
            operator7,
            operator8
        );


        //////////   ОКНА   ////////// 

        // 1) Факультет базовой инженерной подготовки
        /*
            "Факультет базовой инженерной подготовки"
            "Базалық инженерлік дайындық факультеті"
            "Faculty of Basic Engineering Training"
        */
        var window1 = WindowAggregate.Create(queue.Id, 1, operator1.Id);

        // 2) Школа бизнеса и предпринимательства
        /*
            "Школа бизнеса и предпринимательства",
            "Бизнес және кәсіпкерлік мектебі",
            "School of Business and Entrepreneurship"
        */
        var window2 = WindowAggregate.Create(queue.Id, 2, operator2.Id);

        // 3) Школа машиностроения
        /*
            "Школа машиностроения"
            "Машинажасау мектебі"
            "School of Mechanical Engineering"
        */
        var window3 = WindowAggregate.Create(queue.Id, 3, operator3.Id);

        // 4) Школа технологий атомной и традиционной энергетики
        /*
            "Школа технологий атомной и традиционной энергетики",
            "Атомдық және дәстүрлі энергетикалық технологиялар мектебі",
            "School of Nuclear and Traditional Energy Technologies"
        */
        var window4 = WindowAggregate.Create(queue.Id, 4, operator4.Id);

        // 5) Школа информационных технологий и интеллектуальных систем
        /*
            "Школа информационных технологий и интеллектуальных систем"
            "Ақпараттық технологиялар және зияткерлік жүйелер мектебі"
            "School of Information Technology and Intelligent Systems"
        */
        var window5 = WindowAggregate.Create(queue.Id, 5, operator5.Id);

        // 6) Школа архитектуры и строительства
        /*        
            "Школа архитектуры и строительства"
            "Сәулет және құрылыс мектебі"
            "School of Architecture and Construction"
        */
        var window6 = WindowAggregate.Create(queue.Id, 6, operator6.Id);

        // 7) Школа металлургии и обогащения полезных ископаемых
        /*
            "Школа металлургии и обогащения полезных ископаемых ",
            "Металлургия және пайдалы қазбаларды байыту мектебі",
            "School of Metallurgy and Mineral Processing"
        */
        var window7 = WindowAggregate.Create(queue.Id, 7, operator7.Id);

        // 8) Школа наук о земле 
        /*
            "Школа наук о Земле"
            "Жер туралы ғылымдар мектебі"
            "School of Earth Sciences"
        */
        var window8 = WindowAggregate.Create(queue.Id, 8, operator8.Id);

        var windows = new List<WindowAggregate>()
        {
            window1,
            window2,
            window3,
            window4,
            window5,
            window6,
            window7,
            window8
        };

        context.Windows.AddRange(windows);

        //////////   УСЛУГИ   ////////// 

        string[][] serviceNames = new string[][]
        {
            new string[] // 1
            {
                "Факультет базовой инженерной подготовки",
                "Базалық инженерлік дайындық факультеті",
                "Faculty of Basic Engineering Training"
            },
            new string[] // 2
            {
                "Школа бизнеса и предпринимательства",
                "Бизнес және кәсіпкерлік мектебі",
                "School of Business and Entrepreneurship"
            },
            new string[] // 3
            {
                "Школа машиностроения",
                "Машинажасау мектебі",
                "School of Mechanical Engineering"
            },
            new string[] // 4
            {
                "Школа технологий атомной и традиционной энергетики",
                "Атомдық және дәстүрлі энергетикалық технологиялар мектебі",
                "School of Nuclear and Traditional Energy Technologies"
            },
            new string[] // 5
            {
                "Школа информационных технологий и интеллектуальных систем",
                "Ақпараттық технологиялар және зияткерлік жүйелер мектебі",
                "School of Information Technology and Intelligent Systems"
            },
            new string[] // 6
            {
                "Школа архитектуры и строительства",
                "Сәулет және құрылыс мектебі",
                "School of Architecture and Construction"
            },
            new string[] // 7
            {
                "Школа металлургии и обогащения полезных ископаемых ",
                "Металлургия және пайдалы қазбаларды байыту мектебі",
                "School of Metallurgy and Mineral Processing"
            },
            new string[] // 8
            {
                "Школа наук о Земле",
                "Жер туралы ғылымдар мектебі",
                "School of Earth Sciences"
            },
        };

        for (int i = 1; i <= 8; i++)
        {
            var windowIndex = i - 1;
            var names = serviceNames[windowIndex];

            var service = ServiceAggregate.Create(
                queue.Id,
                name: LocalizedString.Create(
                    new List<CultureString>() {
                            new(Culture: "ru", Value: names[0]),
                            new(Culture: "kk", Value: names[1]),
                            new(Culture: "en", Value: names[2])
                    }
                ).Value,
                literal: null
            );

            var bachelorService = service.CreateChild(
                name: LocalizedString.Create(
                    new List<CultureString>() {
                        new(Culture: "ru", Value: "Бакалавриат"),
                        new(Culture: "kk", Value: "Бакалавриат"),
                        new(Culture: "en", Value: "Bachelor"),
                    }
                ).Value,
                literal: ServiceLiteral.Create($"B{i}").Value
            ).Value;

            bachelorService.AddWindowLink(windows[windowIndex].Id);

            var magistracyService = service.CreateChild(
                name: LocalizedString.Create(
                    new List<CultureString>() {
                        new(Culture: "ru", Value: "Магистратура"),
                        new(Culture: "kk", Value: "Магистратура"),
                        new(Culture: "en", Value: "Magistracy"),
                    }
                ).Value,
                literal: ServiceLiteral.Create($"M{i}").Value
            ).Value;

            magistracyService.AddWindowLink(windows[windowIndex].Id);

            var doctorateService = service.CreateChild(
                name: LocalizedString.Create(
                    new List<CultureString>() {
                        new(Culture: "ru", Value: "Докторантура"),
                        new(Culture: "kk", Value: "Докторантура"),
                        new(Culture: "en", Value: "Doctorate"),
                    }
                ).Value,
                literal: ServiceLiteral.Create($"D{i}").Value
            ).Value;

            doctorateService.AddWindowLink(windows[windowIndex].Id);

            context.Services.AddRange(
                service,
                bachelorService,
                magistracyService,
                doctorateService
            );
        }


        //////////   ТЕРМИНАЛЫ   ////////// 

        var terminal = TerminalAggregate.Create(queue.Id, "Терминал для приемной комиссии 2023", "1");
        context.Terminals.Add(terminal);

        context.SaveChanges();


        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 1; i <= windows.Count; i++)
        {
            stringBuilder.AppendLine($"Окно № {i}: https://hq.ektu.kz/window/{windows[i - 1].Id.Value} | {serviceNames[i - 1][0]}");
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"Терминал: https://hq.ektu.kz/terminal/{terminal.Id.Value}");
        stringBuilder.AppendLine($"Табло: https://hq.ektu.kz/tablo/{queue.Id.Value}");

        var client = new HttpClient();

        client.PostAsJsonAsync(
            "https://api.telegram.org/bot<token>/sendMessage",
            new {
               chat_id = "<chatId>",
               text = stringBuilder.ToString() 
            }
        ).Wait();
    }
}