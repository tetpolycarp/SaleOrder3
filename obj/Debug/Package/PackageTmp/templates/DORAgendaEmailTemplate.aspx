<script runat="server">

    Protected Sub Page_Load(sender As Object, e As EventArgs)

    End Sub
</script>


<html>
<head>
    <meta charset="utf-8" />
    <title></title>
    <style type="text/css">
          .myTextarea
            {
                 width: 100% !important;
                 min-width: 100%;
                 height: 200%;
                 min-height: 150%;
            }     
             .auto-style7 {
                 font-size: small;
             }
        .auto-style15 {
            font-size: large;
            height: 40px;
        }
            
        .auto-style19 {
            font-size: medium;
            color: #FFFFFF;
        }
        .auto-style21 {
            font-size: x-small;
        }
        .auto-style32 {
            font-size: small;
            text-align: left;
            margin: 1px 1px;
       
        }
        .auto-style33 {
            color: #FF3300;
            font-weight: bold;
            font-size: medium;
            text-decoration: underline;
        }
        .auto-style34 {
            color: #FF3300;
            font-weight: bold;
            font-size: medium;
            
        }
     
       
     
        .auto-style35 {
            font-size: small;
            text-align: left;
            color: #FF3300;
        }
     
       
     
        .auto-style36 {
            width: 20%;
            height: 23px;
        }
        .auto-style37 {
            font-size: small;
            text-align: left;
            margin: 1px 1px;
            height: 23px;
        }
     
       
     
        .auto-style38 {
            color: #0066FF;
            border: 4px solid tomato;
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
                                <span style="font-size: 24pt; font-family: Arial, sans-serif; color: rgb(102, 102, 102);">   Enterprise Change Management<o:p></o:p></span></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

 <br />  <br />
        
        <table>
        <tr>
             <td align="left" class="auto-style15"><strong>DOR Agenda ~~~ScheduleDate~~~ <span class="auto-style21">~~~Identity~~~</span> </strong> </td>
        </tr>
             <tr>
             <td align="left" class="auto-style15"><strong><span class="auto-style21">View <a href="http://scmconsoles.staging.int/AdminPages/DORAgenda">DOR Agenda Console</a> for further details</span> </strong> </td>
        </tr>
     </table>
     <br />
    <br />
       <table style="text-align: right; vertical-align: top; width: 100%;"  class="auto-style7">
                         <tr>
                             <td style="text-align: right; vertical-align: top; width: 20%;" ><strong>Meeting Info:</strong></td>
                             <td align="center">
                                 <table style="width: 100%;">
                                      <tr  align="center">
                                         <td class="auto-style33">
                                            Conference Room
                                         </td>
                                         <td class="auto-style33">
                                            Audio Information

                                         </td>
                                         <td class="auto-style33">
                                            WebEx Live Meeting
                                         </td>
                                     </tr>
                                     <tr align="center">
                                         <td class="auto-style34">
                                             ~~~ConferenceRoom~~~
                                         </td>
                                         <td class="auto-style34">
                                            ~~~AudioInformation~~~
                                         </td>
                                         <td>
                                             <a href="~~~Webex~~~">Join WebEx meeting</a>
                                         </td>
                                     </tr>
                                   
                                 </table>
                                 <br />
                                 </td>
                         </tr>
           <tr>
               <td>
                   <br />
               </td>
               <td></td>
           </tr>
            <tr>
                        <td style="text-align: right; vertical-align: top; width: 20%;"><strong>Today's Incidents:</strong></td>
                        <td class="auto-style32">
                              ~~~TodayIncident~~~     
                        </td>

                    </tr>
             <tr>
                        <td style="text-align: right; vertical-align: top; " class="auto-style36"><strong>Change Requests:</strong></td>
                        <td class="auto-style37">
                               ~~~ChangeRequest~~~
                        </td>

                    </tr>
             <tr>
                        <td style="text-align: right; vertical-align: top; width: 20%;"><strong>Scheduling Restrictions:</strong></td>
                        <td class="auto-style35">
                                <em>~~~SchedulingRestriction~~~    </em>
                         </td>

                    </tr>
                    <tr>
                        <td><br /></td>
                    </tr>
                     </table>
   <br /><br />
      <table align="center" style="width: 95%;">
          <tr><td style="text-align: center">
              <h2 class="auto-style38">~~~summary~~~</h2><br />
              </td></tr>
          </table>
    <table align="center" style="width: 95%;">
    
        <tr align="center" bgcolor="#008BC3">
             <td class="auto-style19"><strong></strong></td>
             <td class="auto-style19" style="width: 8%"><strong>ID</strong></td>
            <td class="auto-style19"><strong>Product Name</strong></td>
                <td class="auto-style19"><strong>Env</strong></td>
              <td class="auto-style19"><strong>Region</strong></td>
              <td class="auto-style19" style="width: 8%"><strong>Status</strong></td>
             <td class="auto-style19"><strong>Change Type</strong></td>
                <td class="auto-style19" style="width: 8%"><strong>Start Time</strong></td>
                <td class="auto-style19"><strong>Group Involved</strong></td>
              <td class="auto-style19"><strong>Requestor</strong></td>
               <td class="auto-style19"><strong>Notes</strong></td>
            </tr>

        ~~~StartRowPlaceHolder~~~
        <tr align="center" class="auto-style32" bgcolor="~~~rowbackgroundcolor~~~">
            <td>~~~Index~~~</td>
            <td>~~~ID~~~</td>
            <td>~~~ProductName~~~</td>
            <td>~~~Env~~~</td>
            <td>~~~Region~~~</td>
             <td>~~~Status~~~</td>
             <td>~~~ChangeType~~~</td>
            <td>~~~StartTime~~~</td>
            <td>~~~GroupInvolved~~~</td>
              <td>~~~Requestor~~~</td>
            <td>~~~Notes~~~</td>
          </tr>
        ~~~EndRowPlaceHolder~~~


    </table>


</body>
</html>