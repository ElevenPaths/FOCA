
<p align="left">
<img src="./doc/FOCA_White.jpg"/>
</p>

# FOCA (Fingerprinting Organizations with Collected Archives)

**FOCA  is a tool used mainly to find metadata and hidden information in the documents it scans**. These documents may be on web pages, and can be downloaded and analysed with FOCA.

It is capable of analysing a wide variety of documents, with the most common being **Microsoft Office**, **Open Office**, or **PDF** files, although it also analyses Adobe InDesign or SVG files, for instance.

These documents are searched for using three possible search engines: **Google**, **Bing**, and **DuckDuckGo**. The sum of the results from the three engines amounts to a lot of documents. It is also possible to add local files to extract the EXIF information from graphic files, and a complete analysis of the information discovered through the URL is conducted even before downloading the file.

## ✔️ Requisites

To run the solution locally the system will need:

* An instance of **SQL Server**.

#### 📝Notes

* When starting the app the system will check if there is a **SQL Server** instance available. If none is found the system will prompt a window for introducing a connection string.  

## 📜 License

[GNU GENERAL PUBLIC LICENSE](https://www.gnu.org/licenses/gpl-3.0.en.html)

## ☕ Further reading 

* https://www.elevenpaths.com/labstools/foca/index.html
* https://0xword.com/es/libros/59-pentesting-con-foca.html
