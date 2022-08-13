using appui.shared;
using HtmlAgilityPack;
using System.Data;
using Xunit;

namespace appui.tests
{
    public class WebDocumentTests
    {
        [Fact]
        public void CreatingNewInstance_DocumentIsNull_DefaultDocumentIsNotNull()
        {
            var document = new WebDocument(null);
            Assert.NotNull(document);
        }

        [Fact]
        public void GetConnections_DocumentIsNull_EmptyList()
        {
            var document = new WebDocument(null);
            var connections = document.GetConnections();

            Assert.Empty(connections);
        }

        [Fact]
        public void GetConnections_DocumentIsEmpty_EmptyList()
        {
            HtmlNode node = HtmlNode.CreateNode("<html>");
            var document = new WebDocument(node.OwnerDocument);
            var connections = document.GetConnections();

            Assert.Empty(connections);
        }

        [Fact]
        public void CreatingNewInstance_DocumentWithValidDocument_WebDocumentInstanceCreated()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml("<html><div id='divContent'></div></html>");
            var document = new WebDocument(htmlDocument);

            Assert.NotNull(document);
        }

        [Fact]
        public void GetConnections_DocumentWithValidHtml_OneConnection()
        {
            string html = @"<html>
                                <div id='divContent'>
                                    <a class='toggle-vis'>
                                        ver_0
                                    </a>
                                    <table id='TestInfrastructure'>
                                        <tbody>
                                            <tr id='client_id_0' role='row'>
                                                <td class='namespace sorting_1'>client_name</td>
                                                <td>client_id_0</td>
                                                <td data-site='sitename'>
                                                    <font class='namespace' data-client-id='client_id_0'>database_name</font>
                                                    <br>
                                                    <small class='dbServerVersion'>[server_name - 2022]</small>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </html>";

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var document = new WebDocument(htmlDocument);

            var connection = document.GetConnections().ElementAtOrDefault(0);

            Assert.NotNull(connection);

            Assert.Equal("ver_0", connection.Version);
            Assert.Equal("client_id_0", connection.Id);
            Assert.Equal("client_name", connection.Client);
            Assert.Equal("database_name", connection.Database);
            Assert.Equal("server_name", connection.Server);
        }
    }
}