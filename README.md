
 <a name="Content"></a>
### Содержание
- [Содержание](#Content)
- [Описание основного шаблона](#General)  
- [Импорт аналитики и шаблон событий](#Analytics)
- [Сборка билда](#Build)
- [Дополнительные полезные шаблоны](#Templates)  
- [Полезные скрипты](#OtherScripts)  
- [Список изменений](#Changelist)  

 <a name="General"></a>
### Основной шаблон

В Template Scene -> RegularTemplate висят обязательные Observer и LevelManager. RegularTemplate должен быть в каждой сцене. Так же в префаб RegularTemplate отправляются все компоненты и объекты необходимые в каждой сцене-уровне.  
Observer является связующим компонентом между игровыми сущностями и\или интерфейсом со списком всех событий в игре.  

- Основа Observer :  
IsGameLaunched - флаг, который возвращает true с момента вызова OnGameStarted и до момента вызова OnWinLevel\OnLoseLevel.  
OnGameStarted - событие, срабатывает по нажатию кнопки-туториала (описано ниже).  
OnWinLevel - событие, которое необходимо вызывать при финише\выигрыша сцены. После вызова вылетает панель победы (и другое в зависимости от подписок).  
OnLoseLevel - событие, которое соответственно необходимо вызывать при проигрыше. После вызова вылетает панель проигрыша (и другое в зависимости от подписок).  
CallOnWinLevel - метод исключающий множественные вызовы события OnWinLevel, предпочтительнее пользоваться им.  
CallOnWinLevel(float delay) - метод вызывающий OnWinLevel с заданной задержкой и исключающий множественные вызовы.  
Аналогичные методы присутствуют и для события OnLoseLevel.  

- Подписка на события :  
Observer.Instance.OnGameStarted += StartPlay;  
Observer.Instance.OnWinLevel += SomeMethod;  
Observer.Instance.OnWinLevel += delegate { SomeMethod(2f); };  

 - Вызов событий :  
Observer.Instance.OnWinLevel();  
Observer.Instance.CallOnWinLevel();  
Observer.Instance.CallOnLoseLevel(5f);  

 - Основа LevelManager :  
Загрузка сцен производится со стартовой сцены Preload, необходимой для префаба аналитики и других неуничтожаемых объектов при загрузке сцен.  
Все уровни - все сцены имеющиеся в build settings - сначала грузятся по порядку по мере прохождения, далее после прохождения каждого уровня по 1 разу - уровни грузятся в рандомном порядке.  
Чтобы уровни всегда запускались зацикленно по порядку нужно в LevelManager установить LoadType на Linear.  
Чтобы уровни всегда запускались рандомно нужно в LevelManager установить ScenesLoadType на Random.  
Чтобы поменять загружаемую при запуске игры сцену (в эдиторе) нужно в RegularTemplate -> LevelManager вписать индекс нужной сцены и нажать Set current scene.  
Чтобы сбросить прогресс до нулевой сцены нужно нажать Reset progress (сбросит прогресс, не сработает в случае если ScenesLoadType == Random).  

 - В сцене висит Canvas с панелью победы\проигрыша и двумя туториал кнопками (рука + восьмерка\тап ту плей).  
Одну из кнопок необходимо удалить\отключить в зависимости от типа управления в игре.  
Кнопка-туториал растянута на весь экран и по нажатию отключается и вызывает событие OnGameStarted.  
В панели уже настроены кнопки рестарта сцены и загрузки следующей сцены.  
 
 - В файле GameConstants пишутся все константы игры как строковые так и численные (пр-р теги, имена пулов, префсы), а в AnimatorHashes по примеру список всех стейтов\переменных из всех аниматоров.
    - быстрый доступ к часто использующимся повторяющимся данным (особенно в случае с аниматором).  
    - доступ к ключевым константам игры без поиска в коде.  
    - быстрая и безболезненная смена повторяющихся строковых\численных значений.  
.

 - ObjectPooler'у лучше делегировать всю работу со спавном, чтобы проще было тречить какие объекты где спавнятся и когда кому пропадать.
    - спавн с автовозвратом через n сек, спавн объектов рандомно, спавн объектов со взвешенным рандомом.  
.

 - В проекте имеется дженерик синглтон, благодаря которому синглтоны можно создавать просто наследуясь от Singletone< T>.  
 Прим. :  
 public class SomeClass : Singleton< SomeClass> {}  
 
 - В проекте имеется ассет DOTween, с помощью которого можно легко реализовывать простые и сложные анимации движения.  
 Например чтобы заставить объект двигаться к заданной точке в течение 1 секунды достаточно одной строки кода transform.DOMove(position, 1f);  
 
 - В проекте имеются пак мультяшных шейдеров и эффектов по адресу Content -> Toony Colors Pro и Content -> Cartoon FX.  
 
 - В папке Keystore находится ключ для подписи публикующихся билдов.  
 
 <a name="Analytics"></a>
### Импорт аналитики и шаблон событий

После импорта пакейджа аналитики в проект нужно решить зависимости.  
Для iOS Assets->External Dependency Manager->iOS Resolver->Install Cocoapods.  
Для Android Assets->External Dependency Manager->Android Resolver->Force Resolve.  
Вписать ключи и апп айди VoodooPackages->Tiny Sauce->Edit settings.  
Затем добавить префаб аналитики в Preload сцену.  
В LevelManager в методах Start и LoadNextScene нужно раскомментировать строки вызова методов TinySauce аналитики.  
Далее открыть файл Templates->Analytics->AnalyticsEventSender, раскомментировать код и добавить скрипт в префаб RegularTemplate.  

 <a name="Build"></a>
### Сборка билда

После настройки аналитики заполнить в Project settings->Player иконку, имя компании и приложения.  
Подписать приложение ключом в Project settings->Publishing settings. Ключ с инструкцией и паролями находится в папке keystore.  
Выставить в Project settings->player->Other settings->Configuration->Scripting backend на IL2CPP и ниже в target architectures галочки напротив ARMv7 и ARMv64 (требования гугл плей).  
Выписать из Facebook Settings (Facebook->Edit settings)  
 - Название пакета Google Play (Package Name)  
 - Название класса (Activity Facebook) (Class Name)  
 - Ключевые хэш-адреса (Debug Android Key Hash)  
.

 <a name="Templates"></a>
### Дополнительные полезные шаблоны - Assets/Templates

 - ProgressBars Templates  
**Полезности** - canvas\world space прогресс\стейдж бары с легкой сменой направления независимо от роста\убавления прогресса  
В папке Assets/Templates/ProgressBars Templates есть примеры  
Подробное описание по ссылке https://github.com/KonstantKuz/ProgressBars-Templates  

 - ObjectPooler  
**Полезности** - спавн с автовозвратом через n сек, спавн объектов рандомно, спавн объектов со взвешенным рандомом.  
В папке Assets/Templates/ObjectPooler/Example есть пример  
Подробное описание по ссылке https://github.com/KonstantKuz/ObjectPooler  

 - ScreenshotMaker  
Рантайм эдитор скриншотер, делающий скрины в разных разрешениях за раз.  
Исп. : кинуть в сцену префаб, добавить нужные разрешения в окне GameView и в ScreenshotMaker, поставить галочку MakeShot напротив каждого нужного разрешения, запустить игру.  

 - Popup Text  
World space всплывашка с простенькой popup анимацией.  
Пр. ObjectPooler.Instance.SpawnObject("Popup").GetComponent< PopupText>().SetPopup("+1", Color.red);  

 - StimulText  
Canvas space всплывашка с 3 анимациями.  

 - ReorderableListExtensions  
 Позволяет легко рисовать удобные списки вместо дефолтных  
 Подробное описание по ссылке https://github.com/KonstantKuz/ReorderableList-Extension
 
  <a name="OtherScripts"></a>
 ### Полезные скрипты - Assets/Scripts
 
 - InputManager.cs
Универсальный класс детекта тапов от игрока. Умеет работать как с событиями мыши для standalone, так и с событиями тачей для девайсов.
1. метод IsTapped() - возвращает true, если игрок в данный фрейм тапнул по экрану
2. свойство TapPhase - возвращает фазу тапа/клика
3. свойство MoveVector - возвращает нормализованный вектор смещения тапа/клика по сравнению с предыдущим фреймом (фактически, это направление свайпа).
4. свойство Swipe - возвращает тип свайпа в текущий момент. если в текущий момент времени свайпов нет, возвращает Swipe.NONE  

- UsefulMethods  
1. UsefulMethods.Instance.DelayedCall - вызов метода\события с задержкой.  
Пример : UsefulMethods.Instance.DelayedCall(2f, SomeMethod);
1. UsefulMethods.Remap - приведение значения из одного промежутка значений к значению из другого промежутка значений.  
Пример : необходима зависимость размера объекта от количества условного ресурса. тогда :  
float size = UsefulMethods.Remap(resource, resourceMin, resourceMax, sizeMin, sizeMax);
 
  <a name="Changelist"></a>
### Список изменений
    
 - {+ UPD 18.12.20 +}
    - добавил контейнер полезных методов UsefulMethods.cs.  
    - убрал пак эффектов.  
    - обновил настройки проекта.  
    .
 - {+ UPD 07.12.20 +}
    - добавил обработчика основных игровых событий по адресу Templates->Analytics.  
    - добавил раздел импорта аналитики и сборки билда.  
    .
 - {+ UPD 02.12.20 +}
    - добавил в Observer методы вызова выигрыша\проигрыша с задержкой.  
    - добавил в Observer флаг IsGameLaunched.  
    - добавил в LevelManager возможность запускать уровни рандомно после первого последовательного прохождения уровней, а так же только последовательно\только рандомно.  
    - поставил повыше кнопку рестарта в панели проигрыша.  
     