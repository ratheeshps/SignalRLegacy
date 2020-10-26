<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SignalRDemo._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.6.4.min.js"></script>
    <script src="Scripts/jquery.signalR-2.4.1.min.js"></script>
    <script src="/signalr/hubs"></script>
    <script type="text/javascript">
        $(function () {

            var job = $.connection.messageHub;
            job.client.displayNotification = function () {
                getMessages();
            };
            $.connection.hub.start();
            getMessages();

        });
        function getMessages() {
            var tbl = $('#tbl');
            $.ajax({
                url: 'default.aspx/GetData',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                success: function (data) {
                    debugger;
                    if (data.d.length > 0) {
                        var newdata = data.d;
                        tbl.empty();
                        tbl.append(' <tr><th>ruleViolationId</th><th>ruleId</th><th>transactionId</th></tr>');
                        var rows = [];
                        for (var i = 0; i < newdata.length; i++) {
                            rows.push(' <tr><td>' + newdata[i].ruleViolationId + '</td><td>' + newdata[i].ruleId + '</td><td>' + newdata[i].transactionId + '</td></tr>');
                        }
                        tbl.append(rows.join(''));
                    }
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
          <table id="tbl"></table>
        <div>
        </div>
    </form>
</body>
</html>
