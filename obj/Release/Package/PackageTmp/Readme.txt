﻿Check all the credential in OneNote "BTV Church"
Credential to log into front end console: username: admin/chau, pass: tong nha


Steps to do for the new year: Read note above. 
Note: new good feature, whenever user create invoice on desktop or mobile, it will sends the SMS text message as the receipt and create the new user.
Whenever, register new user, it will send SMS text message to the phone number.

From AWS Console (west region, Oregon)
0. Create the AMI which is the last good EC2 instance. Only keep it for 1 month during Banh Chung
1. Start EC2 instance "btv-saleorders-server"
2. From "CloudWatch\Events\Rules", make sure to enable these 2 rules: 
     "StartBtvEc2Instance" which start VM at 15:00 UTC daily (which is 7am local time)
     "StopBtvEc2" which stop VM at 7:00 UTC daily (which is 11pm local time)
3. From "EC2\Elastic IP addresses", create the new ElasticIPs.
   Then associate this new IP with the EC2 instance. I will loose the current RDP to this, and need to switch to the new IP to RDP


From the VM
RDP and log into the VM using the new IP from the Elastic IP address above ("administrator: secure", if try the generate pwd from AWS console, it probably won't work)
Backup admin user: chau - secure
Watch out if the local admin user is expired (which it should not be, because the user set to be none expired). Then we have to re-enter IIS App pool and task scheduler for the new pwd, otherwise, they won't work
Therefore, confirm if the local admin pwd never expired.
Test the site before modify anything: http://<new ip>
0. Make the copy of "C:\inetpub\wwwroot" then compile the project solution to make sure build "C:\Users\Administrator\source\repos\SaleOrder2".
1. In VS, search for any text in the previous year "2025", change to "2026" and the content in the codes base on that make change like "Xuân XXXX", change all the logo and text to new year by compiling the visual studio project again. 
   look for logo file name "snake_2025.jpg" (tricky, maybe the previous replacement for "2024" to "2025" might change the name to "snake_2026.jpg") 
   In VS, right click "SaleOrderProject" project then select "Publish". Using "Staging" profile, then try to publish and test to "wwwroot_staging" first.
2. Update web.config to point to the new database folder: <add key="RootDataDirectory" value="C:\GianHangTet2026" />
   In this new database folder, there is "SaleItems.json". So update the items and price correctly for each year.
3. On the Webserver VM, re-enable the Task Scheduler job "SaleOrder_SyncDbJsonFiles". Update "C:\SyncDbFiles.bat", to sync and archive the database folder onto tetpolycarp google drive
   - Update up "C:\SyncDbFiles.bat"
   - Update the script to format as yyyymmdd so we can easily sort instead
   - Update the "target" and "source" variables. 
   - In Google drive, create the new folder there for new year, say  "G:\My Drive\2026"
   - Then make sure to update the file "SyncDbFiles.bat"" in Project Solution for version control (probably the text replace in step #1 already did it)
   - Run the Task Scheduler, then go to google drive to verify if the file copy up there. It should be in "tetpolycarp" account, look in onenote for pwd.
4. Update Phone contact in InvoiceMobileReceipt.aspx and InvoicePrint.aspx

From Google drive: Idea is that we want to update G3:G4, H3:H4 cell in the sheet
Note for 2025. use the tutorial video below try to fix these methods: UpdateTotalPickupToInventoryInGoogleSheet() >> Methods.UpdateRangeValueInSheet() >> GetCredentialService()
Which read the .json file somehow.
As from the test in 2024, it work when running the code "locathost", but fail when running through IIS. Use the video below to try again.

Purpose: Manually copy the googlesheet from the previous year to the current year to track for inventory. This sheet is used in SalesOrder software, "Inventory" on the menu.
1. On drive.google.com, from "tetpolycarp" account, go to the new year "2026", create folder name "InventorySheet-UsedByChauSoftware" (the previous task scheduler execution above should already create "2026")
2. Now, go back to the previous year folder, "2025\InventorySheet-UsedByChauSoftware", then make the copy of this sheet "BanhChung_Inventory_2026". Then move it to "2025\InventorySheet-UsedByChauSoftware". Make sure to rename it correctly
   In "BanhChungInventory", update all the date and clear the data. Make sure the calculated fields in the first yellow table looks good.
   In "OtherInventory", update/edit "Mat Hang". It is a list in the rule. If update the item, need to update the calculate field in the first yellow table as well.
   Also, there is the date validation rule for Column F, might need adjust the date too.
3. After copying, open the sheet "BanhChung_Inventory_2026" > File > Share > Set share "everyone with the link", make sure the share link is "Editable"
   Also, reminder whoever update Inventory, if they use on mobile, need to have google sheet app.
4. Copy the shared link to notepad
5. Go back to Visual Studio, Open "SharedComponents\SharedMethods.cs", look for method name "UpdateTotalPickupToInventoryInGoogleSheet()". 
6. Update variable name "spreadsheetId" with the ID. Note, it's only the GUID of the sheet, which is part of string in step #4. Also update the sample https in the line above.
7. Update the variable "expiredDate" date in the same method.
8  Open "Inventory.aspx", then update the full string in step #4 in (line 52) for "iframe > src"
9  On the same file (line 68) perform as followed. When update for Mobile, get the different link. Go to the sheet on google, then select "File > Share > Public to Web" > "Link" tab for "Entire Document" and "Web page", then select Publish
   to get the link in there to get the entire "iframe" codes
10. Open "Inventory.aspx.cs", then update the full string in step #4 in (line 74) for the "Redirect" function
Step to publish google sheet using iFrame: http://googledrive.in30minutes.com/google-sheets-embed-live-spreadsheet/
Step to Google Sheet API:
1. Setup API key: https://console.developers.google.com/apis/dashboard
    Tutorial: https://www.youtube.com/watch?v=shctaaILCiU&list=UUm5pREiVM6hwsZKqJww8XuA&index=21
2. 






Perform Sms Text
0. Change the message call something as "E-receipt" when sending out the message to customer.
1. Send SmS text message. Purchase the service from "twilio.com" for $20 in 2023 (Maybe need to charge more). Use their API and make a method call "SendNewSmsTextMessage()". 
2. Check OneNote for logon account info. 
   If the account go into "dormancy", because it's inactive after a long time, then will need to "Resolve" and activate it again.
3. Get a free number and activtate/register it, then we can start sending the sms text message
4. Optional Hassle: To Purchase a Then log into "twilio.com", got to "Develop > Phone Number > Manage > buy a number", look for 714 area code and buy a number for $1.15/month
5. From Twilio main page, 3 things need to get, "AccountSID", "Auth Token" and "Twilio phone number" (either the 1866 one or the one we purchase. Make sure it register and activate)
6. Then in "SharedMethods.cs", update "SendNewSmsTextMessage" method
   - Look for "accountSid" and "authToken"
        Everytime create the new invoice, send sms text.
        Everytime create the new user, send sms text.
        Reminder function for each invoice
   - 2024/1/18 Painful to realize that, when the phone number is not approve, if the body of the text content the URL or https, it won't send out.
     (Only needs to update these below if the phone is not approve yet)
     In SendSmsTextMessage.aspx.cs, Page_Load, line 24-26, update those variables to exclude the URL at the end
     In Invoice.aspx.cs, line 1969, "NewInvoice", update message variable to exclude the URL at the end
     InvoiceAdmin.aspx.cs, line 1914
     InvoiceModbile.aspx.cs, line 1498
     Register.aspx.cs, line 103, fix the link

   - We can also view the Messages logs in twilio: "Click on the Active Numbers", there is a tab call Message log


Database Related
1. Since SQL Express moved locally to this machine (Azure SQL was expensive), 
2. Open up SQL Management Studio
3. In VS "SaleOrderProject", copy the content of "Backup_sql_tables\CreateBtvUsersDB.sql" as SQL query
4. Execute the query to create the new Database "BtvUsers_New"
5. From the GianHangTet front end website "http://localhost/Default", login as "admin", tong nha to make sure the session still active
6. Now rename the current database "BtvUsers" to "BtvUser_Old"
7. Swap rename "BtvUsers_New" to "BtvUsers". This database should have all table but empty.
8. Quickly go "http://localhost/Account/Register" then create new user "admin", tong nha, with all the permission roles
9. Then continue to create all the users from "user account" sheet in google drive "2024"
It should send SMS message to all the users, make sure to test the software and send them the final text to show how to login.
10. If want to re-create the users and give them more permission, the easier way is look for the user in "AspNetUsers", make Update query to change the name of the user.
    Then go to the front end, and recreate the user again, make sure to give whatever the roles they want.
    Note, using their same account and phone number as password. Use my phone number instead, so they won't get the SMS text again.


2. Read the backlog below


Items need to be done after Banh Chung done 2024:
0. Leave it for a few more days
1. Shutdown EC2 instance. 
2. Turn off cloudwatch to start/stop EC2 instance: "CloudWatch\Rules\StartBtvEc2Instance" and "StopBtvEc2"
   Release ElasticIPs from EC2 instance then delete it.
3. Cancel/release the active phone number from the console
4. Submit the support ticket from the console to ask them to suspend/cancel the account
5. Export all invoice/tracking sheet into excel then upload to google drive for archiving. Look back in previous year for reference. ("tetpolycarp" account, look into onenote for pwd) 
6. Delete the old AMI (deregister AMI, then go to snapshot and delete it)

Backlogs:
list after 2025:
- Make the Summary report to read google sheet, the Inventory tab, for all the ban le item, then include it in the table. So we dont have to manually keep sync-up again. Or do the opposite to write to google sheet, but I think the challenge last time is when writting to the sheet, instead we read easier.
- Make another summary report page to give out only the total banh chung/banh test for each complex Invoice, so we can easily view the big account, or save after we're done with the project
- Order detail, troubleshoot on the print selected button, that should print out many invoice at the same time. This is useful when we need to print those item that need to pickup during the day, so people can pack.

list after 2024:
2024's note: Bug in OrderDetail page, the overall top table, somehow the pickup/not pickup data is wrong. Show the table in the form again, then debug and troubleshoot with 2024 data
Suggestion for 2025 - Last few days, should always create 1 buffer invoice for 100 chung, 100 tet for spare
- In Audit > Invalid: exclude those invoice that already completed. Perhaps, make sub-menu for invalid completed and incompleted.
- Change the invoice to start as "001" instead of "101". Make sure to default the number to be 3 digits. Otherwise, some codes might fail
- Make "Audit Invoice" only available for some sort of admin to view
- Whenever invoice create/update, fix "UpdateTotalPickupToInventoryInGoogleSheet()" to update Inventory google sheet.
  Instead of just adding the new chung/tet, run through all the invoice to add up the total chung/tet again to make the data more accurate.
  Do the same thing for other Inventory items as well. Maybe store the data in google sheet or maybe store the data in our web form.
- Old backlog: Update Inventory google sheet:
    - Make another column to track Tong so goi, subtract so mat trong google sheet
    - Fix software to update "Tong so banh Pickup", tong so banh con lai
    - Make new calculation table to give out the live date for tong so banh lam, subtract (banh order + banh pickup), give out the number
- For those invoices that "cancelled", won't show them in report or for cast inventory.
- Tracking sheet, if the #5 is selected and completed, the system should be smart enough to expand and select #6 open. Save the select of "Giao" check box into json as well. So next time we can load it up.
- On the Desktop Invoice Page, whenever change the quantity, should turn on AutoPost back to update the page.
- When Enter the quantity, refresh post back on invoice page
- Make a way to easily track TNTT sale. Either use Good sheet, have column for Checkout amount, return amount, so the amount sold. Then convert that in to the invoice. 
   might need to have another radio button for TNTT and make sure the mobile doesn't break. Might be a 1 more radio button for TNTT. When it loads, fillup with all available sale items.


Steps to setup the new Web Server to run:
1. Install Visual Studio and SQL Management Studio
2. Pull the sources as above
3. Use SQL Management Studio to create an empty database "SaleOrderUsers". This database is used to store user identities.
   Go to the codes of Account\Register to uncomment the codes to check if the current user have to be Global Admin to browse the page. So we can create the new user that way
4. Change the "DefaultConnection" string in web.config to match with new server. Also, read more for further notes.
	Also, in webconfig, pay attention on   <add key="RootDataDirectory" value="C:\GianHangTet2024" />
	This is used to store all the database file. The database is flat and be all .json files
5. Run VS in admin mode then start SaleOrder.sln. Try to build and make sure to run locally first. 
	Go to RegisterPage to add admin like http://localhost:61429/Account/Register
6. Install IIS: Control Panel -> Turn Window features on or off -> Internet Information Services (Web Management Tools, WWW services)
	Test: http://localhost/
7. From Project in VS, using the existing Profile to publish the codes to C:\inetpub\wwwroot
8. Make Window Task Scheduler to sync .json data files daily. The idea is that, copy folder like "C:\GianHangTet2024" to "C:\GoogleDrive\2022\DatabaseBackup-UsedByChauSoftware".
Note: the reason why we're doing this because we want to make new folder backup everyday. 