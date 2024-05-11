Feature: Processamento de Mensagem SQS
    As a system
    I want to process SQS messages
    So that I can manage pedidos

    Scenario: Processar Mensagem SQS
        Given I have a MensagemSQS
        When I call ProcessarMensagemSQS
        Then the MensagemSQS should be processed
