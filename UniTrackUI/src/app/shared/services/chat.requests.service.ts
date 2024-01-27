import { Injectable } from '@angular/core';
import { Statistic } from '../models/statistic';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { UserService } from './user.service';
import { RecommendedMaterial } from '../models/recommended-material';
import { UserResult } from '../models/user-result';
import { LocalStorageKeys } from '../enums/local-storage-keys.enum';
import { MessageResult } from '../models/message-result';

@Injectable({
  providedIn: 'root',
})
export class ChatRequestsService {

  constructor(private http: HttpClient, private userService: UserService){}

  getContacts(): Observable<UserResult[]> {
    // Retrieve USER_ID from local storage
    const userId = localStorage.getItem(LocalStorageKeys.USER_ID);
  
    // Check if USER_ID is available
    if (!userId) {
      // Handle the case where USER_ID is not available
      console.error('USER_ID not found in local storage');
      // You might want to return an empty observable or handle the error in a different way
      return of([]);
    }
  
    // Remove double quotes from USER_ID
    const formattedUserId = userId.replace(/"/g, '');
  
    // Make the HTTP request using the formatted USER_ID
    return this.http.get<UserResult[]>('http://localhost:5036/api/chat/people/' + formattedUserId, { withCredentials: true });
  }

  getMessagesInChat(receiverId:string): Observable<MessageResult[]>{
    const userId = localStorage.getItem(LocalStorageKeys.USER_ID);
    if (!userId) {
      // Handle the case where USER_ID is not available
      console.error('USER_ID not found in local storage');
      // You might want to return an empty observable or handle the error in a different way
      return of([]);
    }
    const formattedUserId = userId.replace(/"/g, '');
    const formattedReceiverId = receiverId.replace(/"/g, '');
    const params = {
      senderId: formattedUserId,
      receiverId: formattedReceiverId
    };
    return this.http.get<MessageResult[]>('http://localhost:5036/api/chat/messagesInChat',{ 
      params: params,
      withCredentials: true 
    })
  }

}

