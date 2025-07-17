<%@ Page Title="PlayList Converter" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="GenfromYTMobile.aspx.cs" Inherits="PlayListCreator_FW.Pages.GenfromYTMobile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            background-image: url('<%= ResolveUrl("~/Images/radio-cassette.png") %>');
            background-size: cover;
            background-repeat: repeat;
            background-attachment: fixed;
            background-position: center -25px;
            left: 0;
            background-color: rgba(105, 105, 105, 0.1);
        }


    </style>

     <script type="text/javascript">
        $(function () {
            var updateCount = $.connection.updateCount;

            // This function is called from server to update progress
            updateCount.client.updateCount = function (countAdded, total) {
                $('#lblCountSongsInserted').text(countAdded + " songs added of " + total);
            };
            $('#lblCountSongsInserted').css('color', 'black');

            // Start the connection
            $.connection.hub.start().done(function () {
                var connectionId = $.connection.hub.id;
                console.log("SignalR connected with ID: " + connectionId);

                // Set hidden field value
                $('#<%= hfConnectionId.ClientID %>').val(connectionId);
            });

            // Set up onclick event handler for the button
            $('#<%= ButtontoSpotify.ClientID %>').on('click', function () {
                $('#lblCountSongsInserted').text('Starting export...');
            });

        });
     </script>

    <div class="container">
        <!-- YouTube Playlist -->
       <div style="background-color: rgba(255,255,255,0.7); padding: 15px; border-radius: 8px;
            max-width: 500px; width: 100%;">
            <label for="txtPlaylistYT" style="display: block; margin-bottom: 5px;"><strong>YouTube Playlist Link:</strong></label>
            <asp:CustomValidator
                ID="ValidateYTLink"
                runat="server"
                ControlToValidate="txtPlaylistYT"
                Display="Dynamic"
                ErrorMessage=""
                ForeColor="Red"
                ValidateEmptyText="true" />
            <asp:TextBox ID="txtPlaylistYT" runat="server" style="width: 100%;" CssClass="form-control" />
        </div>
          <!-- Spotify Playlist -->
         <div style="background-color: rgba(255,255,255,0.7); padding: 15px; border-radius: 8px;
           max-width: 500px; width: 100%; margin-top:5px">
            <label for="txtPlaylistYT" style="display: block; margin-bottom: 5px;"><strong>Spotify Playlist Link:</strong></label>
             <asp:CustomValidator
                ID="ValidateSpotifyLink"
                runat="server"
                ControlToValidate="txtPlaylistYT"
                Display="Dynamic"
                ErrorMessage=""
                ForeColor="Red"
                ValidateEmptyText="true" />
            <asp:TextBox ID="txtPlayListSpotify" runat="server" style="width: 100%;" CssClass="form-control" />
             <asp:Label ID="lblCountSongsInserted" runat="server" ClientIDMode="Static"></asp:Label>
        </div>

        <div style="display: flex; flex-direction: column; gap: 10px; ">
            <asp:Button ID="ButtonCSV" runat="server" Text="Download Youtube List to CSV"
                CssClass="btn btn-success rounded-pill px-4" OnClick="ButtonCSV_Click"  style="margin-top:5px;"/>
            <asp:Button ID="ButtontoSpotify" runat="server" Text="Export to Spotify Playlist"
                CssClass="btn btn-danger rounded-pill px-4"
                OnClick="ButtontoSpotify_Click"  style="margin-top:5px;"/>
        </div>
        <asp:HiddenField ID="hfConnectionId" runat="server" />
</div>

   <div class="jumbotron text-center bg-primary text-white py-5 rounded"  style="margin-top: 10px; background-color: rgba(255,255,255,0.9);">
      <h1 class="display-4">Intructions</h1>
       <p class="lead">1. Open the Youtube app</p>
        <p class="lead">2. Go to the playlist, find the Share button and tap on it</p>
        <p class="lead">3. Then Copy the Playlist link</p>
        <p class="lead">4. Come back here and paste the link on the YouTube playlist address bar above</p>
        <p class="lead">5. If you just want to export to CSV tap the Download to CSV button. You will download the playlist in CSV.</p>
        <p class="lead">6. To export to Spotify, open the Spotify app and go to the playlist you want to import the songs</p>
        <p class="lead">7. Find the 3 dots and tap on it, then tap on Share and Copy link</p>
        <p class="lead">8. Come back here and paste the link on the Spotify playlist address bar above</p>
        <p class="lead">9. Tap Export to Spotify. You will be redirected to Spotify's authentication page to allow access to this webapp to modify the playlist. </p>
        <p class="lead">10. After allowing access to your playlist, return to this page if not automatically re-directed.
            Note: This app does not store any information
            about your playlist or acount, other then temporary access to import the Youtube items to your Spotify playlist. </p>
        <p class="lead">11. Enjoy</p>
    </div>


</asp:Content>
