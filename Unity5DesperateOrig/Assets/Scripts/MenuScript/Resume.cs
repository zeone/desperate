using UnityEngine;
using System.Collections;

public class Resume
{
    public string brithData = "Год рождения: 12.09.1994 г.";
    public string surname = "Тукмачёв";
    public string name = "Виталий";
    public string patronymic = "Юрьевич";
    public string school = "Школа №9 г.Севастополь, дата окончания 2012 г.";
    public string college = "СевВУССиИТ, дата окончания 2014 г.";
    public string profession = "Оператор по обработке информации и программного обеспечения";
    public string graduateWork = "Создание приложений для Windows и других платформ";
    public float ratingDiploma = 10.9f;
    public string[] knowledge = { "Unity 4 (Game Engine)","IDE: VisualStudio (System control version)", "MonoDevelop",
                                    "3DMax","Vray","Iray","Zbrush","Coat-3d","PhotoShop","SubstanceDesigner","ShetchBook",
                                    "Office(All Program)","CorelDraw"};
    public void Specification()
    {
        string aboutMe = "Я увлекся IndeGameDev еще в 9 классе, первая игра была написана на..." + 
            "Прошу прощения, не помню на чем, но там был транспорт и физика, от чего у меня захватывало тогда дух." + 
            "Так вот, с того времени, начал изучение всего!" +
            /*Левел дизайн, свето дизайн, моделинг, текстурирование, анимация, CAT Анимация, Ригинг, создание спрайтов,
             * разработка UI и его дизайн, функциональность, логика игры, написание кода, AI, базы данных XML, Пост обработка,
             * написание шейдеров, структура проекта, подготовка звука, методы оптимизации.
             */
            "оцените мои умения Вы!, буду рад излить все эти знания.";
        string myBenefits = "Почему я? Я знаю все аспекты разработки от идей до завершения, смогу понять каждого сотрудника," +
            "поставив себя на его место. Изобретателен, от части мечтатель. И может Вы, исполнители мой мечты.";
            //P.S. Не имею вредных привычек, чуть ли не забыл ссылки  на скриншоты моих работ в методе MyWork
    }


    public  void MyWork()
    {
        string URL = "https://vk.com/album119501112_186711841";
    }
}
