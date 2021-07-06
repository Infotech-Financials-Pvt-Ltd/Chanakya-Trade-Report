# Chanakya Trade Report

[![beta](https://img.shields.io/badge/beta_version-1.0.0-yellow)](https://github.com/Chanakya-Infotech/Chanakya-Trade-Report/releases/tag/v1.0.0-beta)

## About Chanakya Trade Report application:
The National Stock Exchange has released the Online Trade Inquiry Service (NOTIS) APIs to allow their trading members to view their daily trade reports.

Chanakya Trade Report is a free open source software created by Infotech Financials Pvt Ltd which uses the NOTIS API to display daily trade reports of Equity, Derivatives and Currency markets. It is a desktop application that requires members to input their username and tokens assigned by NSE in order to function.

![trade-window-with-data](/Chanakya-Trade-Report/docs/trade-report-window.jpg)


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.


### Prerequisites

.NET Framework Runtime 4.7.2 or higher.

```
Windows 7, 8, 8.1 and 10

.NET Framework 4.7.2 or .NET Framework 4.8 (latest)

Visal Studio 2019 IDE (any edition)

```

### Installing

For development:
Install Visual Studio IDE (this will install .NET framework)

For running app only:
Install .NET Framework Runtime to run this app exe only.

## Deployment

- To run the application and download Trade data, the machine's Static IP must be configured with exchange.

1. Run the Chanakya-Trade-Report.exe file created in the Release folder under the bin directory inside the project folder.<br>
Example : D:\Infotech\Chanakya-Trade-Report\Chanakya-Trade-Report\bin\x64\Release\Chanakya-Trade-Report.exe
 
2. Copy the Release to the desired windows machine whose Static IP is configured with exchange and run the Chanakya-Trade-Report.exe file.

3. Download setup installer from [Releases](https://github.com/Chanakya-Infotech/Chanakya-Trade-Report/releases) and install the application on the machine.<br>
Start Chanakya Trade Report application using the option in Start Menu or by double clicking on Desktop shortcut.

## Running the tests

- Open the project in Visual Studio and Build the program.
- Run the Chanakya-Trade-Report.exe file created in the Release folder under the bin directory inside the project folder.
- Enter required login and configuration details in settings window for trade data.
- Trade data and total calculations will be displayed in the window.

## Documentation

### Table of contents:
- [Login](#login)
- [Settings](#settings)
- [Trade Report Window](#trade-report-window)
- [Important Files](#important-files)
- [App Settings File](#app-settings-file)
- [Trade Download File](#trade-download-file)
- [Error Messages](#error-messages)
- [Notes](#notes)

## Run Chanakya-Trade-Report.exe 

## Login
### A login window will open for account details

![](/Chanakya-Trade-Report/docs/login-window.jpg)

## Enter market and account details

![](/Chanakya-Trade-Report/docs/login-details-entered.jpg)

Select Market ID from the dropdown and enter proper user details.

The Username and Password is currently configured in code.

## Settings
### A settings window will open

![](/Chanakya-Trade-Report/docs/settings-window-empty.jpg)

## Enter proper details in settings window

These details will be used to communicate with exchange to request and
receive trade data.

![](/Chanakya-Trade-Report/docs/settings-details.jpg)

The `Key` and `Secret` field values will be received from exchange and will
be used to acquire access token from exchange.

If Access Token already generated it can be used in Access Token field.

`Login URL` : This URL will be used to send request for Access Token.

`Trade URL`: This URL will be used to send request for receiving Trade
data.

`Trade Data Directory` : This location in the system will be used to save
Trade.txt files.

The `Browse` button will provide a dialog box to select a folder.

`Trade Interval` : This is the time interval in seconds between trade
download request sent to exchange.

`Records Limit` : This value will show the number of Trade entries in the
table.

`Actions URL` : This URL will be used to send request for receiving
Actions data.

The `Debug` checkbox if checked will create a file in which all json
request and responses of a session will be saved for debugging.

`All` Checkbox : To receive data for all trades.

`TM` Checkbox : To receive only data of clearing members.

`Trades` Checkbox : To receive trade data.

`Actions` Checkbox : To receive actions data.

`Old Format` Checkbox : To save trade data in old Trade.txt format.

`New Format` Checkbox : To save trade data in new format in API.

On updating these settings the values will be saved on the system and will be loaded automatically from
previous session when app restarts.

## Trade Report Window

![](/Chanakya-Trade-Report/docs/trade-report-window.jpg)

This window will display the downloaded trade data in a table.

Following fields are displayed in the window:

`Total Buy Value` : Total value of trades on buy side.

`Total Sell Value` : Total value of trades on sell side.

`Total Trade Value` : Total of buy and sell value.

`Total Trades` : Total of buy and sell traded quantity.

`Total Count` : Total number of trade entries received from exchange.

`Last Updated` : Latest time at which the Trade.txt file is updated.

`Refresh Button` : To reload table data.

## Important Files
## Log File

The log files are generated as per market selected in login along with
date in name.

These files will be generated in the same folder where application is
installed.

Example:

C:\\Users\\mseva\\Desktop\\Chanakya-Trade-Report\\log-FO-2021-06-17.txt

![](/Chanakya-Trade-Report/docs/log-file-location.jpg)

![](/Chanakya-Trade-Report/docs/log-data.jpg)

## Sequence Number and Message ID File

This `sequence number` of latest trade received and the `message Id` of last
request is saved in a file so that if the application is closed during a
session it could be used when app restarts.

This file is created as per market segment along with date in the name.

Example:
C:\\Users\\mseva\\Desktop\\Chanakya-Trade-Report\\last-FO-seq-no-15062021.txt

File Data: 453659,2

Here `453659` is the last received trade sequence number and `2` is the
message Id used to make the download request.

![](/Chanakya-Trade-Report/docs/lat-seq-no.jpg)

## App Settings File

## Chanakya-Trade-Report.exe.config

The user settings in the application are saved in a file and loaded when
app starts.

This file is located in the same folder where application is installed.

The data in this file is saved in XML format.

![](/Chanakya-Trade-Report/docs/user-settings-file.jpg)


![](/Chanakya-Trade-Report/docs/user-settings-file-data.jpg)


## Trade Download File

The trade data will be downloaded to files at the folder location
specified in settings window.

The file name will contain market type and date.

Example :

If the folder mentioned in settings window is named Trade Data in
Desktop directory :

File path –

For Future & Options market:

C:\\Users\\mseva\\Desktop\\Trade Data\\TradeFO\_02062021.txt

For Cash market:

C:\\Users\\mseva\\Desktop\\Trade Data\\TradeCM\_02062021.txt

![](/Chanakya-Trade-Report/docs/trade-download-folder.jpg)

## Error Messages

The error messages will be shown using message box popups and the
detailed error will be added in the log file.

![](/Chanakya-Trade-Report/docs/error-message-popup.jpg)

## NOTES

-   The trade interval time in settings should not be less than 30
    seconds as currently mentioned by the exchange. Failing to obey this
    rule shall make them remove user from whitelist.

-   Do not move or delete the Trade.txt file generated when application
    is running.

-   For restarting app to download from 1st trade kindly move the
    downloaded Trade.txt files and last received sequence number files
    from setup folder to different location.

-   Do not edit or remove any other files present in the setup folder.

## Cloning Project from Github:
1.	Go to Github repository link of project on web browser.
2.	You will see a Code with Download sign button. Click that button.
3.	A https link will be displayed this link will be used to clone the project.
4.	Open Visual Studio go to Team Explorer tab there you will see clone option link.
5.	Click that link it will ask for remote repository link.
6.	Paste the copied link from web browser and press enter.
7.	The project will be downloaded on local machine.
8.	Another option to clone project is by using Github Desktop application. On clicking the Code with Download sign button on browser you will also see Open with Github Desktop option.
9.	On clicking that option Github Desktop application will open and display a window where to download the project.

#### NOTE: You can also download the source code as zip file and open the project in Visual Studio on your machine.

## License

This project is licensed under the Microsoft Public License (MS-PL) License - see the [LICENSE.md](LICENSE.md) file for details

## Feedback

If you just have some feedback or suggestions you can [create an issue](https://github.com/Infotech-Financials-Pvt-Ltd/Chanakya-Trade-Report/issues), [start a discussion](https://github.com/Infotech-Financials-Pvt-Ltd/Chanakya-Trade-Report/discussions) or email us directly at [support-ctr@infofin.com](mailto:support-ctr@infofin.com)

