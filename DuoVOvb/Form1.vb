Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description
Imports System.ServiceModel.Security
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports DuoVOvb.eMedNySOAPClient

Public Class Form1

    Private Function setBinding() As CustomBinding
        Dim binding As CustomBinding = New CustomBinding()

        Dim security As AsymmetricSecurityBindingElement = AsymmetricSecurityBindingElement.CreateMutualCertificateDuplexBindingElement()
        security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256Sha256
        security.IncludeTimestamp = True
        security.AllowSerializedSigningTokenOnReply = True
        binding.Elements.Add(security)
        binding.Elements.Add(New TextMessageEncodingBindingElement(MessageVersion.Soap11WSAddressing10, Encoding.UTF8))
        binding.Elements.Add(New HttpsTransportBindingElement With {
                .RequireClientCertificate = True,
                .KeepAliveEnabled = False,
                 .AllowCookies = False
            })
        Return binding
    End Function

    Public Function GetCertificateFromStore(certName As String) As X509Certificate2

        Dim store As New X509Store(StoreLocation.CurrentUser)
        Try
            store.Open(OpenFlags.ReadOnly)

            Dim certCollection As X509Certificate2Collection = store.Certificates
            Dim currentCerts As X509Certificate2Collection = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, False)
            Dim signingCert As X509Certificate2Collection = currentCerts.Find(X509FindType.FindBySubjectName, certName, False)

            If signingCert.Count = 0 Then
                Return Nothing
            End If
            Return signingCert(0)
        Finally
            store.Close()
        End Try
    End Function

    Private Function setEndPoint() As EndpointAddress
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Dim epUri As Uri = New Uri("http://www.w3.org/2005/08/addressing/anonymous?oin=00000001800866472000")
        Dim endPoint As EndpointAddress = New EndpointAddress(epUri, EndpointIdentity.CreateDnsIdentity("xml-test.duo.nl"))
        Return endPoint
    End Function

    Private Function setServiceCertificate() As X509Certificate2
        Dim x509Service As New X509Certificate2("xml-test.duo.nl.cer")
        Return x509Service
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim cc As MHSClient.DUO_VOInformatiediensten_IdentificerenPersoonVO_V10_PortTypeClient = New MHSClient.DUO_VOInformatiediensten_IdentificerenPersoonVO_V10_PortTypeClient(setBinding(), setEndPoint())
            cc.ClientCredentials.ClientCertificate.Certificate = GetCertificateFromStore("vovavo.duo.nl")
            cc.ClientCredentials.ServiceCertificate.DefaultCertificate = setServiceCertificate()
            cc.Endpoint.Behaviors.Add(New ClientViaBehavior(New Uri("https://xml-test.duo.nl:6940/vo-webservices/")))
            cc.Endpoint.Contract.ProtectionLevel = Net.Security.ProtectionLevel.Sign
            cc.Endpoint.Behaviors.Add(New CustomValidationBehavior())

            Dim req As New MHSClient.aanleverenIdentificerenPersoonVO_Request
            req.identificatiecodeBedrijfsdocument = Guid.NewGuid().ToString()
            req.verzendendeInstantie = "00AB"
            req.ontvangendeInstantie = "DUO"
            req.datumTijdBedrijfsdocument = DateTime.Now
            req.geboorteDatum = "2000-09-18"
            req.geslacht = MHSClient.Geslachtv02.Item1

            Dim zPGN As New MHSClient.ZonderPGN
            zPGN.achternaam = "test"
            zPGN.voornamen = "Demo"
            req.Item = zPGN

            Dim binnen As New MHSClient.BinnenlandsAdres
            binnen.straatnaam = "Kempkensberg"
            binnen.huisnummer = "12"
            binnen.postcode = "9722TB"
            binnen.plaatsnaam = "GRONINGEN"
            zPGN.Item = binnen

            Dim res As New MHSClient.aanleverenIdentificerenPersoonVO_Response

            res = cc.aanleverenIdentificerenPersoonVO(req)
        Catch ex As Exception
            RichTextBox1.Text = ex.Message

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try
            Dim cc = New MHSClient.DUO_VOInformatiediensten_IdentificerenPersoonVO_V10_PortTypeClient()
            cc.Endpoint.Contract.ProtectionLevel = Net.Security.ProtectionLevel.Sign
            cc.Endpoint.Behaviors.Add(New CustomValidationBehavior())

            Dim req As New MHSClient.aanleverenIdentificerenPersoonVO_Request
            req.identificatiecodeBedrijfsdocument = Guid.NewGuid().ToString()
            req.verzendendeInstantie = "00AB"
            req.ontvangendeInstantie = "DUO"
            req.datumTijdBedrijfsdocument = DateTime.Now
            req.geboorteDatum = "2000-09-18"
            req.geslacht = MHSClient.Geslachtv02.Item1

            Dim zPGN As New MHSClient.ZonderPGN
            zPGN.achternaam = "test"
            zPGN.voornamen = "Demo"
            req.Item = zPGN

            Dim binnen As New MHSClient.BinnenlandsAdres
            binnen.straatnaam = "Kempkensberg"
            binnen.huisnummer = "12"
            binnen.postcode = "9722TB"
            binnen.plaatsnaam = "GRONINGEN"
            zPGN.Item = binnen

            Dim res As New MHSClient.aanleverenIdentificerenPersoonVO_Response

            res = cc.aanleverenIdentificerenPersoonVO(req)

        Catch ex As Exception
            RichTextBox1.Text = ex.Message
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cc As New DUO_VOAanlevering_InschrijvingVO_V1.DUO_VOAanlevering_InschrijvingVO_V10_PortTypeClient(setBinding(), setEndPoint())
        cc.ClientCredentials.ClientCertificate.Certificate = GetCertificateFromStore("vovavo.duo.nl")
        cc.ClientCredentials.ServiceCertificate.DefaultCertificate = setServiceCertificate()
        cc.Endpoint.Behaviors.Add(New ClientViaBehavior(New Uri("https://xml-test.duo.nl:6940/vo-webservices/")))
        cc.Endpoint.Contract.ProtectionLevel = Net.Security.ProtectionLevel.Sign
        cc.Endpoint.Behaviors.Add(New CustomValidationBehavior())

        Dim req As New DUO_VOAanlevering_InschrijvingVO_V1.AanleverenInschrijvingVO_Request

        req.identificatiecodeBedrijfsdocument = Guid.NewGuid().ToString()
        req.verzendendeInstantie = "29ZT"
        req.ontvangendeInstantie = "DUO"
        req.datumTijdBedrijfsdocument = Format(DateTime.UtcNow, "yyyy-MM-dd").ToString & "T" & Format(DateTime.UtcNow, "HH:mm:ss").ToString & "Z"
        req.ItemElementName = DUO_VOAanlevering_InschrijvingVO_V1.ItemChoiceType.burgerservicenummer
        req.Item = "712004166"
        req.brin = "29ZT"
        req.inschrijvingvolgnummer = "123456"
        req.datumInschrijving = "2017-11-12"
        req.datumUitschrijving = "2018-11-12"


        Dim inschrijvingPeriodeVOList = New List(Of DUO_VOAanlevering_InschrijvingVO_V1.InschrijvingPeriodeVO)
        Dim inschrijvingPeriodeVO_Data = New DUO_VOAanlevering_InschrijvingVO_V1.InschrijvingPeriodeVO
        inschrijvingPeriodeVO_Data.datumBegin = "2017-11-12"
        inschrijvingPeriodeVO_Data.leerjaar = "7"
        inschrijvingPeriodeVO_Data.opleidingcode = "0015"
        inschrijvingPeriodeVO_Data.vestiging = "29ZT00"
        inschrijvingPeriodeVO_Data.indicatieBekostigbaar = True

        'Dim AanvullendeOpleidingList = New List(Of DUO_VOAanlevering_InschrijvingVO_V1.AanvullendeOpleiding)
        'Dim AanvullendeOpleiding = New DUO_VOAanlevering_InschrijvingVO_V1.AanvullendeOpleiding
        'AanvullendeOpleiding.opleidingcode = "DFDFFDF"
        'AanvullendeOpleidingList.Add(AanvullendeOpleiding)
        'inschrijvingPeriodeVO_Data.aanvullendeOpleiding = AanvullendeOpleidingList.ToArray

        'Dim ExperimentList = New List(Of DUO_VOAanlevering_InschrijvingVO_V1.Experiment)
        'Dim Experiment = New DUO_VOAanlevering_InschrijvingVO_V1.Experiment
        'Experiment.naam = "DFDFFDF"
        'ExperimentList.Add(Experiment)
        'inschrijvingPeriodeVO_Data.experiment = ExperimentList.ToArray

        inschrijvingPeriodeVOList.Add(inschrijvingPeriodeVO_Data)

        req.inschrijvingPeriodeVO = inschrijvingPeriodeVOList.ToArray

        'Dim OntwikkelingsperspectiefList = New List(Of DUO_VOAanlevering_InschrijvingVO_V1.Ontwikkelingsperspectief)
        'Dim Ontwikkelingsperspectief = New DUO_VOAanlevering_InschrijvingVO_V1.Ontwikkelingsperspectief
        'Ontwikkelingsperspectief.datumBegin = "2019-01-02"
        'OntwikkelingsperspectiefList.Add(Ontwikkelingsperspectief)

        'req.ontwikkelingsperspectief = OntwikkelingsperspectiefList.ToArray


        'Dim VerblijfAndereInstellingList = New List(Of DUO_VOAanlevering_InschrijvingVO_V1.VerblijfAndereInstelling)
        'Dim VerblijfAndereInstelling = New DUO_VOAanlevering_InschrijvingVO_V1.VerblijfAndereInstelling
        'VerblijfAndereInstelling.datumBegin = "2019-01-02"
        'VerblijfAndereInstelling.brin = "3434"
        'VerblijfAndereInstelling.vestiging = "NL"
        'VerblijfAndereInstelling.verblijfsoort = "JJ4"
        'VerblijfAndereInstellingList.Add(VerblijfAndereInstelling)

        'req.verblijfAndereInstelling = VerblijfAndereInstellingList.ToArray

        Try
            Dim res As New DUO_VOAanlevering_InschrijvingVO_V1.AanleverenInschrijvingVO_Response
            res = cc.aanleverenInschrijvingVO(req)
        Catch ex As Exception
            RichTextBox1.Text = ex.Message
        End Try




    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim myDocument As New XmlDocument
        myDocument.LoadXml(RichTextBox2.Text)
        myDocument.Schemas.Add("namespace here or empty string", "D:\Bestanden\project SVPO\PvE VO\VO XSD\VOA Inschrijvingen\DUO_VOAanlevering_InschrijvingVO_V1.xsd")
        Dim eventHandler As ValidationEventHandler = New ValidationEventHandler(AddressOf ValidationEventHandler)
        myDocument.Validate(eventHandler)
    End Sub

    Private Sub ValidationEventHandler(ByVal sender As Object, ByVal e As ValidationEventArgs)
        Select Case e.Severity
            Case XmlSeverityType.Error
                Debug.WriteLine("Error: {0}", e.Message)
            Case XmlSeverityType.Warning
                Debug.WriteLine("Warning {0}", e.Message)
        End Select
    End Sub
End Class
