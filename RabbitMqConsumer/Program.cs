using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://wchpyyvv:xFLMWyo7qrUU2fBHE7wNgq1_L9nzrWUF@toad.rmq.cloudamqp.com/wchpyyvv");

//Bağlantıyı Aktifleştirme ve Kanal Açma
//using kullanıyoruz çünkü bellekte yer etmesin.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region default
////Queue Oluşturma
////durable parametresi mesajların kuyrukta ne kadar kalacağını belirtir.
////exclusive parametresi kuyruğun özel olup olmadığı. Birden fazla kuyruk ile özel olarak işlem yapıp yapamayacağımızı belirtir.
////autoDelete tüm mesajların tüketildiğinde kuyruğun silinip silinemeyeceğine dair yapılanmadır.
////consumer'da da kuyruk publisherdaki ile birebir aynı yapılanmada tanımlanmalıdır.
//channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

////Queue'dan Mesaj Okuma
//EventingBasicConsumer consumer = new(channel);
////autoAck: false ise mesaj onaylama süreci aktifleştirmiş oluruz.
////message acknowledgement
//channel.BasicConsume(queue:"example-queue", autoAck:false, consumer: consumer);
////prefetchSize: mesaj boyutu byte cinsinden. 0 sınırsız demek
////preFetchCount: aynı anda işleme alınabilecek mesaj sayısı
////global: tüm consumerlar için mi yoksa belirli bir consumer için mi
//channel.BasicQos(prefetchSize: 0, prefetchCount: 1,global: false);
//consumer.Received += (sender , e) =>
//{
//    //Kuyruğa gelen mesajın işlendiği yer.
//    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getiriir.
//    //e.Body.Span veya e.Body.ToArray fonksiyonları kuyruktaki mesajın byte verisini getirecektir.
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    //yalnızca bu mesajı doğrulamak için
//    //multiple: false 
//    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);


//};
#endregion

#region DirectExchange

////1.Adım publisher'da ne ise aynısı olması lazım
//channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);
////2.Adım publisher tarafından routerKey'de bulunan değerdeki kuyruğa göndeirlen mesajları kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekmekte.
////bunun için öncelikle bir kuyruk oluşturulmalı.
//string queueName = channel.QueueDeclare().QueueName;

//channel.QueueBind(
//    queue: queueName,
//    exchange: "direct-exchange-example",
//    routingKey: "direct-queue-example"
//    );

//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion


#region FanoutExchange
//string exchange = "fanout-exchange-example";
//channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

//Console.Write("Kuyruk Adınız Giriniz. ");
//string queueName = Console.ReadLine();

//channel.QueueDeclare(queue: queueName,
//                     exclusive: false
//                     );

//channel.QueueBind(
//    queue: queueName,
//    exchange: exchange,
//    routingKey: string.Empty
//    );

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    string message = Encoding.UTF8.GetString(e.Body.Span);
//    Console.WriteLine(message);
//};

#endregion


#region TopicExchange

channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

Console.WriteLine("Dinlenecek topic formatını belirtiniz.");
string topic =Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "topic-exchange-example",
    routingKey: topic
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};
#endregion

Console.Read();