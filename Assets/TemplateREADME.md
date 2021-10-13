
 <a name="Content"></a>
### Содержание
- [Содержание](#Content)
- [Описание основного шаблона](#General)  
- [Импорт аналитики и шаблон событий](#Analytics)
- [Сборка билда](#Build)
- [Дополнительные полезные шаблоны](#Templates)  

 <a name="General"></a>
### Основной шаблон

В Template Scene -> RegularTemplate висят обязательные Observer и LevelManager. RegularTemplate должен быть в каждой сцене. Так же в префаб RegularTemplate отправляются все компоненты и объекты необходимые в каждой сцене-уровне.  
Observer является связующим компонентом между игровыми сущностями и\или интерфейсом со списком всех событий в игре.  

- #### Observer :  
IsGameLaunched - флаг, который возвращает true с момента вызова OnGameStarted и до момента вызова OnWinLevel\OnLoseLevel.  
OnGameStarted - событие, срабатывает по нажатию кнопки-туториала (описано ниже).  
OnWinLevel - событие, которое необходимо вызывать при финише\выигрыша сцены. После вызова вылетает панель победы (и другое в зависимости от подписок).  
OnLoseLevel - событие, которое соответственно необходимо вызывать при проигрыше. После вызова вылетает панель проигрыша (и другое в зависимости от подписок).  
CallOnWinLevel - метод исключающий множественные вызовы события OnWinLevel, предпочтительнее пользоваться им.  

- Подписка на события :  
Observer.Instance.OnGameStarted += StartPlay;  
Observer.Instance.OnWinLevel += SomeMethod;  
Observer.Instance.OnWinLevel += delegate { SomeMethod(2f); };  

 - Вызов событий :  
Observer.Instance.OnWinLevel();  
Observer.Instance.CallOnWinLevel();  

- #### LevelManager :  
Загрузка сцен производится со стартовой сцены Preload, необходимой для префаба аналитики и других неуничтожаемых объектов при загрузке сцен.  
Последовательность уровней прописывается в файле ScriptableData -> LevelsQueue (можно сделать несколько файлов с разными последовательностями уровней. Create -> Levels Queue).
Чтобы уровни всегда запускались зацикленно по порядку нужно в LevelManager установить LoadType на Linear.  
Чтобы уровни всегда запускались рандомно нужно в LevelManager установить ScenesLoadType на Random.  
Чтобы уровни запускались рандомно после одного последовательного прохождения нужно в LevelManager установить ScenesLoadType на RandomAfterLinear. 
Чтобы поменять загружаемую при запуске игры сцену (в эдиторе) нужно в RegularTemplate -> LevelManager вписать индекс нужной сцены и нажать Set current level index.  

- #### ItemSystems :  
Система айтемов включает в себя инвентарь, магазин и выдачу подарков. Предметы в инвентаре могут делиться на группы, могут быть экипируемыми.  
Предметы могут быть доступны для использования изначально, могут покупаться в магазине, могут выдаваться в качестве подарка для использования/для покупки в магазине.  
Для каждого предмета необходимо создать файлы данных для инвентаря, для магазина и для списка подарков.  Пр. смотреть в папке ScriptableData -> ItemSystemsData.  
Созданные файлы предметов для инвентаря должны заполняться в соответствующий предмету файл группы инвентаря, файлы групп инвентаря должны заполняться в список групп в Prefabs -> RegularTemplate -> Inventory -> ItemsGroups.  
Созданные файлы предметов для покупки должны заполняться в соответствующий предмету файл группы покупок, файлы групп покупок должны заполняться  в список групп в Prefabs -> UI -> Shop -> Shop -> Shop -> ItemsGroups и в соответствующий ShopTab. (пр. ExampleShopTab)  
Созданные файлы предметов для выдачи в качестве награды должны заполняться в Prefabs -> RegularTemplate -> GiftsQueue -> ReceivingQueue.  
Обработка и отображение прогресса выдачи подарков реализовано в GiftProgressUpdatePanel.  

- #### AdsManager :  
Для работы с рекламой необходима реализация IAdsImpl, которая передается в AdsManager.  
Перед показом интерстишиал рекламы (AdsManager.Instance.ShowInterstitialAd()) для обработки показа нужно подписаться на событие AdsManager.Instance.onInterAdShowedOrFailed.  
Перед показом ревард видео для обработки награждения игрока нужно подписаться на события AdsManager.Instance.onRewardedAdFailedOrDiscarded и AdsManager.Instance.onRewardedAdRewarded.  


- В папке префабов есть два прогресс бара уровня.  

- В сцене висит Canvas с панелью победы\проигрыша и туториал кнопкой (рука + восьмерка\ в папке с префабами есть тап ту плей).  

Кнопка-туториал по нажатию отключается и вызывает событие OnGameStarted.  
В панели уже настроены кнопки рестарта сцены и загрузки следующей сцены.  
 
- В файле Tags пишутся все переиспользуемые теги, а в AnimatorHashes по примеру список всех стейтов\переменных из всех аниматоров.
    - быстрый доступ к часто использующимся повторяющимся данным (особенно в случае с аниматором).  
    - доступ к ключевым константам игры без поиска в коде.  
    - быстрая и безболезненная смена повторяющихся строковых\численных значений.  
  

- ObjectPooler'у лучше делегировать всю работу со спавном, чтобы проще было тречить какие объекты где спавнятся и когда кому пропадать.
    - спавн с автовозвратом через n сек.  
    - спавн объектов рандомно.  
    - спавн объектов со взвешенным рандомом.  
  

- В проекте имеется дженерик синглтон, благодаря которому синглтоны можно создавать просто наследуясь от Singleton< T>.  
 Прим. :  
 public class SomeClass : Singleton< SomeClass> {}  
 
- В проекте имеется ассет DOTween, с помощью которого можно легко реализовывать простые и сложные анимации движения.  
 Например чтобы заставить объект двигаться к заданной точке в течение 1 секунды достаточно одной строки кода transform.DOMove(position, 1f);  
 
- В проекте имеются пак мультяшных шейдеров и эффектов по адресу Content -> Toony Colors Pro и Content -> Cartoon FX.  
 
- В папке Keystore находится ключ для подписи публикующихся билдов.  
 
 <a name="Analytics"></a>
### Импорт аналитики и шаблон событий

В папке Templates -> Analytics лежат сдк фейсбука и гейм аналитики.
Установить двойным щелчком по пакейджам. 
После установки пакейджей в проект нужно решить зависимости.  
Для iOS Assets->External Dependency Manager->iOS Resolver->Install Cocoapods.  
Для Android Assets->External Dependency Manager->Android Resolver->Force Resolve. 
Затем добавить префаб GameAnalytics в Preload сцену (Window -> GameAnalytics -> Create GameAnalytics Object).

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
 
 - ReorderableListExtensions  
 Позволяет легко рисовать удобные списки вместо дефолтных  
 Подробное описание по ссылке https://github.com/KonstantKuz/ReorderableList-Extension
 