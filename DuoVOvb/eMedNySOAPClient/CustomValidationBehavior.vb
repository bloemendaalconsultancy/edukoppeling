Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher

Namespace eMedNySOAPClient
    Public Class CustomValidationBehavior
        Implements IEndpointBehavior

        Sub Validate(endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate

        End Sub

        Sub AddBindingParameters(endpoint As ServiceEndpoint, bindingParameters As BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters

        End Sub

        Sub ApplyDispatchBehavior(endpoint As ServiceEndpoint, endpointDispatcher As EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior

        End Sub

        Sub ApplyClientBehavior(endpoint As ServiceEndpoint, clientRuntime As ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
            clientRuntime.MessageInspectors.Add(New CustomInspector())
        End Sub
    End Class

End Namespace