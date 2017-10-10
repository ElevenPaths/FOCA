# FOCA

FOCA (Fingerprinting Organizations with Collected Archives) es una herramienta utilizada principalmente para encontrar metadatos e información oculta en los documentos que examina. Estos documentos pueden estar en páginas web, y con FOCA se pueden descargar y analizar.

Los documentos que es capaz de analizar son muy variados, siendo los más comunes los archivos de Microsoft Office, Open Office, o ficheros PDF, aunque también analiza ficheros de Adobe InDesign, o svg por ejemplo.

Estos documentos se buscan utilizando tres posibles buscadores que son Google, Bing y Exalead. La suma de los tres buscadores hace que se consigan un gran número de documentos. También existe la posibilidad de añadir ficheros locales para extraer la información EXIF de archivos gráficos, y antes incluso de descargar el fichero se ha realizado un análisis completo de la información descubierta a través de la URL.

Con todos los datos extraídos de todos los ficheros, FOCA va a unir la información, tratando de reconocer qué documentos han sido creados desde el mismo equipo, y qué servidores y clientes se pueden inferir de ellos.

[![Video sobre Foca](https://img.youtube.com/vi/_SCRceMBI34/0.jpg)](http://www.youtube.com/watch?v=_SCRceMBI34)

Más información en el blog [El otro lado del mal](http://www.elladodelmal.com/2017/10/foca-open-source.html)


## Requerimientos para compilar la solución

* Framework .NET 4.6.1
* Visual Studio 2017 Community
* SQL Server Express Edition
