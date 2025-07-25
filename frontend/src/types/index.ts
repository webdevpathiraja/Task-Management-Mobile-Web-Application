export interface User {
  id: number;
  name: string;
  email: string;
  createdDate: string;
}

export interface Task {
  id: number;
  title: string;
  description: string;
  status: 'Pending' | 'In Progress' | 'Completed';
  userId: number;
  createdDate: string;
  dueDate?: string;
  userName?: string;
}

export interface CreateUserRequest {
  name: string;
  email: string;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  status: 'Pending' | 'In Progress' | 'Completed';
  userId: number;
  dueDate?: string;
}

export interface UpdateTaskRequest {
  title: string;
  description: string;
  status: 'Pending' | 'In Progress' | 'Completed';
  dueDate?: string;
}