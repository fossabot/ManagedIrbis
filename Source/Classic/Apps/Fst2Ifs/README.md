# fst2ifs
Утилита для создания таблицы актуализации

Вниманию сообщества Ирбисоводов предлагается небольшая программа для автоматической генерации таблицы актуализации IFS по имеющейся таблице инвертирования FST. 

Утилита предназначена для автоматического приведения в соответствия IFS-файлов после любого обновления FST-файлов. 

Утилита выполнена как консольная программа и предназначена для запуска из пакетных заданий (BAT-файлов), например, по системному планировщику. 

Программа требует Microsoft.NET Framework 3.5 (можно скачать по адресу http://www.microsoft.com/ru-ru/download/details.aspx?id=22). NET Framework входит в стандартную поставку Windows 7/8 и Windows Server 2008 R2 и может быть установлен посредством апплета Панели управления "Включение компонентов Windows". 

СИНТАКСИС: FST2IFS &lt;input&gt; &lt;output&gt;

Например: FST2IFS IBIS.FST IBIS.IFS 

Выходной файл (в приведенном выше примере IBIS.IFS) каждый раз перезаписывается утилитой заново. Поэтому, чтобы сохранить его предыдущую версию, переместите или переименуйте его. 

В прилагаемом архиве, кроме собственно исполняемого файла утилиты, содержится проект для Microsoft Visual Studio 2008, который позволяет скомпилировать утилиту самостоятельно. 

IFS-файлы появились в версии 2012.1 Ирбис 64. Для пользователей более ранних версий ИРБИС 64 или любых версий ИРБИС 32 данная утилита бесполезна.
