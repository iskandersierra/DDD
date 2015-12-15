using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ModelSoft.Messaging.MessageHandlers
{
    public class DefaultHeadersMessageHandlerTests
    {
        [Fact]
        public async Task DefaultHandlerDoNotAffectMessage()
        {
            // Given a message
            var content = new object();
            var message = new Message();
            var messageSchema = new Uri("http://www.example.com/messageschema");
            message.Schema = messageSchema;
            message.Headers.Add("Key1", "Value1");
            message.Headers.Add("Key2", "Value2");
            message.Content = content;

            // And a context
            var context = new SendMessageContext { Message = message, };

            bool nextWasCalled = false;
            HandleNextMessage next = ctx =>
            {
                // Then the message is the same
                Assert.Same(message, context.Message);

                // And the message schema is the same
                Assert.Equal(messageSchema, message.Schema);

                // And the message content is the same
                Assert.Equal(content, (object)message.Content);

                // And the message headers are the same
                Assert.Equal(2, message.Headers.Count);
                Assert.True(message.Headers.ContainsKey("Key1"));
                Assert.Equal("Value1", message.Headers["Key1"]);
                Assert.True(message.Headers.ContainsKey("Key2"));
                Assert.Equal("Value2", message.Headers["Key2"]);

                nextWasCalled = true;

                return Task.FromResult(true);
            };

            //And a default DefaultHeadersMessageHandler
            var handler = new DefaultHeadersMessageHandler(next);

            // When the handler handles the message
            await handler.SendAsync(context);

            // Then the next handler was called
            Assert.True(nextWasCalled);
        }

        [Fact]
        public async Task SimpleHandlerAffectOnlyGivenHeader()
        {
            // Given a message
            var content = new object();
            var message = new Message();
            var messageSchema = new Uri("http://www.example.com/messageschema");
            message.Schema = messageSchema;
            message.Headers.Add("Key1", "Value1");
            message.Headers.Add("Key2", "Value2");
            message.Content = content;

            // And a context
            var context = new SendMessageContext { Message = message, };

            bool nextWasCalled = false;
            HandleNextMessage next = ctx =>
            {
                // Then the message is the same
                Assert.Same(message, context.Message);

                // And the message schema is the same
                Assert.Equal(messageSchema, message.Schema);

                // And the message content is the same
                Assert.Equal(content, (object)message.Content);

                // And the message headers are the same
                Assert.Equal(3, message.Headers.Count);
                Assert.True(message.Headers.ContainsKey("Key1"));
                Assert.Equal("Value1", message.Headers["Key1"]);
                Assert.True(message.Headers.ContainsKey("Key2"));
                Assert.Equal("Value2", message.Headers["Key2"]);
                Assert.True(message.Headers.ContainsKey("Key3"));
                Assert.Equal("Value3", message.Headers["Key3"]);

                nextWasCalled = true;

                return Task.FromResult(true);
            };

            //And a default DefaultHeadersMessageHandler
            var handler = new DefaultHeadersMessageHandler(next);
            handler.Headers.Add("Key3", "Value3");

            // When the handler handles the message
            await handler.SendAsync(context);

            // Then the next handler was called
            Assert.True(nextWasCalled);
        }

        [Fact]
        public async Task SimpleHandlerDoNotOverrideHeaders()
        {
            // Given a message
            var content = new object();
            var message = new Message();
            var messageSchema = new Uri("http://www.example.com/messageschema");
            message.Schema = messageSchema;
            message.Headers.Add("Key1", "Value1");
            message.Headers.Add("Key2", "Value2");
            message.Content = content;

            // And a context
            var context = new SendMessageContext { Message = message, };

            bool nextWasCalled = false;
            HandleNextMessage next = ctx =>
            {
                // Then the message is the same
                Assert.Same(message, context.Message);

                // And the message schema is the same
                Assert.Equal(messageSchema, message.Schema);

                // And the message content is the same
                Assert.Equal(content, (object)message.Content);

                // And the message headers are the same
                Assert.Equal(3, message.Headers.Count);
                Assert.True(message.Headers.ContainsKey("Key1"));
                Assert.Equal("Value1", message.Headers["Key1"]);
                Assert.True(message.Headers.ContainsKey("Key2"));
                Assert.Equal("Value2", message.Headers["Key2"]);
                Assert.True(message.Headers.ContainsKey("Key3"));
                Assert.Equal("Value3", message.Headers["Key3"]);

                nextWasCalled = true;

                return Task.FromResult(true);
            };

            //And a default DefaultHeadersMessageHandler
            var handler = new DefaultHeadersMessageHandler(next);
            handler.Headers.Add("Key3", "Value3");
            handler.Headers.Add("Key2", "NewValue2");

            // When the handler handles the message
            await handler.SendAsync(context);

            // Then the next handler was called
            Assert.True(nextWasCalled);
        }

        [Fact]
        public async Task OverwritingHandlerDoOverrideHeaders()
        {
            // Given a message
            var content = new object();
            var message = new Message();
            var messageSchema = new Uri("http://www.example.com/messageschema");
            message.Schema = messageSchema;
            message.Headers.Add("Key1", "Value1");
            message.Headers.Add("Key2", "Value2");
            message.Content = content;

            // And a context
            var context = new SendMessageContext { Message = message, };

            bool nextWasCalled = false;
            HandleNextMessage next = ctx =>
            {
                // Then the message is the same
                Assert.Same(message, context.Message);

                // And the message schema is the same
                Assert.Equal(messageSchema, message.Schema);

                // And the message content is the same
                Assert.Equal(content, (object)message.Content);

                // And the message headers are the same
                Assert.Equal(3, message.Headers.Count);
                Assert.True(message.Headers.ContainsKey("Key1"));
                Assert.Equal("Value1", message.Headers["Key1"]);
                Assert.True(message.Headers.ContainsKey("Key2"));
                Assert.Equal("NewValue2", message.Headers["Key2"]);
                Assert.True(message.Headers.ContainsKey("Key3"));
                Assert.Equal("Value3", message.Headers["Key3"]);

                nextWasCalled = true;

                return Task.FromResult(true);
            };

            //And a default DefaultHeadersMessageHandler
            var handler = new DefaultHeadersMessageHandler(next);
            handler.Overwrite = true;
            handler.Headers.Add("Key3", "Value3");
            handler.Headers.Add("Key2", "NewValue2");

            // When the handler handles the message
            await handler.SendAsync(context);

            // Then the next handler was called
            Assert.True(nextWasCalled);
        }

        private Task EmptyNext(SendMessageContext context)
        {
            return Task.FromResult(true);
        }
    }
}
