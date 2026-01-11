<script runat="server">

    Protected Sub Page_Load(sender As Object, e As EventArgs)

    End Sub
</script>


<html>
<head>
    <meta charset="utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style15 {
            font-size: x-large;
            height: 40px;
        }
        .auto-style16 {
            font-size: medium;
        }
        .auto-style17 {
            font-size: large;
            color: #008BC3;
        }
    
        .auto-style19 {
            color: #FFFFFF;
        }
        .auto-style21 {
            font-size: medium;
        }
        .auto-style25 {
            height: 40px;
        }
        .auto-style26 {
            width: 531px;
            height: 30px;
        }
        .auto-style28 {
            font-size: medium;
            text-decoration: underline;
        }
     
       
     
        .auto-style29 {
            width: 25%;
            height: 30px;
        }
        .auto-style30 {
            width: 10%;
            height: 30px;
        }
        .auto-style31 {
            height: 30px;
        }
     
       
     
    </style>
</head>
<body>

    <table border="0" cellpadding="0" class="MsoNormalTable" style="font-family: &quot;Times New Roman&quot;; letter-spacing: normal; orphans: 2; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-style: initial; text-decoration-color: initial;">
        <tr>
            <td style="width: 179px; padding: 0.75pt;" width="5%">
                <p class="MsoNormal" style="margin: 0in 0in 0.0001pt; font-size: 12pt; font-family: &quot;Times New Roman&quot;, serif;">
                    <b><span style="font-size: 36pt; font-family: Arial, sans-serif; color: rgb(0, 139, 195);">Mitchell<o:p></o:p></span></b></p>
            </td>
            <td style="width: 869.25pt; padding: 0.75pt;" width="1159">
                <table border="0" cellpadding="0" class="MsoNormalTable">
                    <tr>
                        <td style="padding: 0.75pt;">
                            <p class="MsoNormal" style="margin: 0in 0in 0.0001pt; font-size: 12pt; font-family: &quot;Times New Roman&quot;, serif;">
                                <span style="font-size: 24pt; font-family: Arial, sans-serif; color: rgb(102, 102, 102);">Software Configuration Management<o:p></o:p></span></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

 <br />  <br />
        
        <table>
        <tr>
             <td align="center" class="auto-style15"><strong>Deployment Schedule ~~~ScheduleDate~~~ </strong> </td>
            <td align="left" class="auto-style15"><strong><span class="auto-style21">~~~Identity~~~</span> </strong> </td>
        </tr>
             <tr>
             <td align="left" class="auto-style15"><strong><span class="auto-style21">View <a href="http://scmconsoles.mitchell.com/AdminPages/DeploymentSchedules">Deployment Schedule</a> for further details</span> </strong> </td>
            <td class="auto-style25" > </td>
        </tr>
     </table>
     <br />
    <br />

    <table style="width: 80%;" align="center">
       
         <tr>
             <td></td>
              <td style="width: 10%;"></td>
            <td  align="left"><strong><span  class="auto-style28">Onshore</span> </strong></td>
              <td  align="left"><strong><span  class="auto-style28">Offshore</span> </strong></td>
        </tr>
         <tr>
           <td align="right" class="auto-style29"><strong><span  class="auto-style16">Primary On-Call :</span></strong></td>
                 <td class="auto-style30"></td>
            <td  align="left" class="auto-style26"><strong> <span class="auto-style17">~~~PrimaryOnCall~~~</span></strong></td>
              <td  align="left" class="auto-style26"><strong> <span class="auto-style17">~~~PrimaryOffshoreOnCall~~~</span></strong></td>
        </tr>
          <tr>
           <td align="right" class="auto-style29"><strong><span  class="auto-style16">Secondary On-Call :</span></strong></td>
                 <td class="auto-style30"></td>
            <td  align="left" class="auto-style31"><strong> <span class="auto-style17">~~~SecondaryOnCall~~~</span></strong></td>
              <td  align="left" class="auto-style31"><strong> <span class="auto-style17">~~~SecondaryOffshoreOnCall~~~</span></strong></td>
        </tr>
    </table>
   <br /><br />
    <table align="center" style="width: 95%;">
    
        <tr align="center" bgcolor="#008BC3">
             <td class="auto-style19"><strong></strong></td>
             <td class="auto-style19"><strong>Platform</strong></td>
            <td class="auto-style19"><strong>Type</strong></td>
                <td class="auto-style19"><strong>ID</strong></td>
              <td class="auto-style19"><strong>Env</strong></td>
              <td class="auto-style19"><strong>Status</strong></td>
             <td class="auto-style19"><strong>Deployment Time</strong></td>
                <td bgcolor="#f48c41" class="auto-style19"><strong>Change Request</strong></td>
                <td bgcolor="#f48c41" class="auto-style19"><strong>Region</strong></td>
              <td bgcolor="#f48c41" class="auto-style19"><strong>Status</strong></td>
               <td bgcolor="#f48c41" class="auto-style19"><strong>Date</strong></td>
                <td class="auto-style19"><strong>Assigned To</strong></td>
                  <td class="auto-style19"><strong>Notes</strong></td>
        </tr>

        ~~~StartRowPlaceHolder~~~
        <tr align="center" bgcolor="~~~rowbackgroundcolor~~~">
            <td>~~~Index~~~</td>
            <td>~~~Platform~~~</td>
            <td>~~~Type~~~</td>
            <td>~~~ID~~~</td>
            <td>~~~Env~~~</td>
             <td>~~~Status~~~</td>
             <td>~~~DeploymentTime~~~</td>
            <td>~~~ChangeRequest~~~</td>
            <td>~~~ChangeRequestRegion~~~</td>
              <td>~~~ChangeRequestStatus~~~</td>
            <td>~~~ChangeRequestDate~~~</td>
             <td>~~~AssignedTo~~~</td>
             <td>~~~Notes~~~</td>
        </tr>
        ~~~EndRowPlaceHolder~~~


    </table>


</body>
</html>