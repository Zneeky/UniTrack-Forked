import { Component } from '@angular/core';

interface ChatEntry {
  personName: string;
  messages: Message[];
}

interface Message {
  text: string;
  sentBy: 'me' | 'friend';
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent {
  people = [
    { name: 'Alice', avatar: 'path/to/avatar1.jpg' },
    { name: 'Bob', avatar: 'path/to/avatar2.jpg' },
    { name: 'Charlie', avatar: 'path/to/avatar3.jpg' },
    { name: 'David', avatar: 'path/to/avatar4.jpg' },
    { name: 'Eva', avatar: 'path/to/avatar5.jpg' },
    { name: 'Frank', avatar: 'path/to/avatar6.jpg' },
    { name: 'Grace', avatar: 'path/to/avatar7.jpg' },
    { name: 'Henry', avatar: 'path/to/avatar8.jpg' },
    { name: 'Ivy', avatar: 'path/to/avatar9.jpg' },
    { name: 'Jack', avatar: 'path/to/avatar10.jpg' },
    { name: 'Katherine', avatar: 'path/to/avatar11.jpg' },
    { name: 'Leo', avatar: 'path/to/avatar12.jpg' },
    { name: 'Mia', avatar: 'path/to/avatar13.jpg' },
    { name: 'Nathan', avatar: 'path/to/avatar14.jpg' },
    { name: 'Olivia', avatar: 'path/to/avatar15.jpg' },
    { name: 'Peter', avatar: 'path/to/avatar16.jpg' },
    { name: 'Quinn', avatar: 'path/to/avatar17.jpg' },
    { name: 'Rachel', avatar: 'path/to/avatar18.jpg' },
    { name: 'Samuel', avatar: 'path/to/avatar19.jpg' },
    { name: 'Tara', avatar: 'path/to/avatar20.jpg' },
    { name: 'Ulysses', avatar: 'path/to/avatar21.jpg' },
    { name: 'Victoria', avatar: 'path/to/avatar22.jpg' },
    { name: 'Walter', avatar: 'path/to/avatar23.jpg' },
    { name: 'Xena', avatar: 'path/to/avatar24.jpg' },
    { name: 'Yasmine', avatar: 'path/to/avatar25.jpg' },
    { name: 'Zane', avatar: 'path/to/avatar26.jpg' },
    { name: 'Anna', avatar: 'path/to/avatar27.jpg' },
    { name: 'Benjamin', avatar: 'path/to/avatar28.jpg' },
    { name: 'Chloe', avatar: 'path/to/avatar29.jpg' },
    { name: 'Dylan', avatar: 'path/to/avatar30.jpg' }
  ];

  chatHistory: ChatEntry[] = [
    { personName: 'Alice', messages: this.generateFakeMessages(6) },
    { personName: 'Bob', messages: this.generateFakeMessages(6) },
    { personName: 'Charlie', messages: this.generateFakeMessages(6) },
    { personName: 'David', messages: this.generateFakeMessages(6) },
    { personName: 'Eva', messages: this.generateFakeMessages(6) },
    // Add more chat history entries as needed
  ];

  selectedChat: ChatEntry = {
    personName: '',
    messages: [] as Message[],
  };

  topRowTranslation = '0'; // Translation for the top row sliding effect
  newMessage: string = '';

  selectChat(personName: string): void {
    // Find the chat entry in chatHistory based on the person's name
    this.selectedChat = this.chatHistory.find(entry => entry.personName === personName) || {
      personName: personName,
      messages: [] as Message[],
    };
  }

  getLastMessage(personName: string): string {
    // Find the chat entry in chatHistory based on the person's name
    const chatEntry = this.chatHistory.find(entry => entry.personName === personName);

    if (chatEntry) {
      // Get the last message from the messages array
      const lastMessage = chatEntry.messages[chatEntry.messages.length - 1];
      return lastMessage ? lastMessage.text : 'No messages';
    } else {
      return 'No messages';
    }
  }

  generateFakeMessages(count: number): Message[] {
    const messages: Message[] = [];
    for (let i = 0; i < count; i++) {
      let text = '';
      switch (i % 6) {
        case 0:
          text = 'Hi there!';
          break;
        case 1:
          text = 'Hello!';
          break;
        case 2:
          text = 'How are you?';
          break;
        case 3:
          text = 'Good!, You?';
          break;
        case 4:
          text = 'Me too';
          break;
        case 5:
          text = 'Ok, bye';
          break;
        default:
          text = 'Default message';
          break;
      }

      messages.push({
        text: text,
        sentBy: i % 2 === 0 ? 'me' : 'friend',
      });
    }
    return messages;
  }

  sendMessage() {
    // Assuming selectedChat is an object with a messages array
    const newMessage: Message = {
      text: this.newMessage,
      sentBy: 'me', // or your user identifier
    };

    // Add the new message to the selectedChat messages array
    this.selectedChat.messages.push(newMessage);

    // Find the chat entry in chatHistory based on the person's name
    const chatEntry = this.chatHistory.find(entry => entry.personName === this.selectedChat.personName);

    if (chatEntry) {
      // Update the messages array for the corresponding chat entry in chatHistory
      chatEntry.messages = this.selectedChat.messages;
    } else {
      // If the chat entry doesn't exist in chatHistory, create a new entry
      this.chatHistory.push({
        personName: this.selectedChat.personName,
        messages: this.selectedChat.messages,
      });
    }

    // Clear the input after sending the message
    this.newMessage = '';
  }
}
