Feature: Consumir Fila e Processar Mensagens
    As a system
    I want to consume the queue and process messages
    So that I can manage pedidos

    Scenario: Consumir Fila e Processar Mensagens
        Given I have a queue with messages
        When I call ConsumirFilaProcessarMensagens
        Then the messages should be processed and deleted from the queue
