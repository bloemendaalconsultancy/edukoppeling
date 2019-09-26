Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Dispatcher

Namespace eMedNySOAPClient
    Public Class CustomInspector
        Implements IClientMessageInspector

        Sub AfterReceiveReply(ByRef reply As Message, correlationState As Object) Implements IClientMessageInspector.AfterReceiveReply

        End Sub

        Function BeforeSendRequest(ByRef request As Message, channel As IClientChannel) As Object Implements IClientMessageInspector.BeforeSendRequest
            request.Headers.From = New EndpointAddress("http://www.w3.org/2005/08/addressing/anonymous?oin=0000000700999A999000")
            Return vbNull
        End Function
    End Class
End Namespace