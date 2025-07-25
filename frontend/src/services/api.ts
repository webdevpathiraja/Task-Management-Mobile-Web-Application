import axios from 'axios';
import type { User, Task, CreateUserRequest, CreateTaskRequest, UpdateTaskRequest } from '../types';

const API_BASE_URL = 'http://192.168.64.77:7000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for logging
api.interceptors.request.use(
  (config) => {
    console.log(`Making ${config.method?.toUpperCase()} request to ${config.url}`);
    return config;
  },
  (error) => {
    console.error('Request error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export const userService = {
  async getAllUsers(): Promise<User[]> {
    const response = await api.get<User[]>('/users');
    return response.data;
  },

  async getUserById(id: number): Promise<User> {
    const response = await api.get<User>(`/users/${id}`);
    return response.data;
  },

  async createUser(user: CreateUserRequest): Promise<User> {
    const response = await api.post<User>('/users', user);
    return response.data;
  },

  async updateUser(id: number, user: CreateUserRequest): Promise<void> {
    await api.put(`/users/${id}`, user);
  },

  async deleteUser(id: number): Promise<void> {
    await api.delete(`/users/${id}`);
  },
};

export const taskService = {
  async getTasksByUserId(userId: number): Promise<Task[]> {
    const response = await api.get<Task[]>(`/tasks/${userId}`);
    return response.data;
  },

  async getAllTasks(): Promise<Task[]> {
    const response = await api.get<Task[]>('/tasks/all');
    return response.data;
  },

  async getTaskById(id: number): Promise<Task> {
    const response = await api.get<Task>(`/tasks/task/${id}`);
    return response.data;
  },

  async createTask(task: CreateTaskRequest): Promise<Task> {
    const response = await api.post<Task>('/tasks', task);
    return response.data;
  },

  async updateTask(id: number, task: UpdateTaskRequest): Promise<void> {
    await api.put(`/tasks/${id}`, task);
  },

  async updateTaskStatus(id: number, status: string): Promise<void> {
    await api.put(`/tasks/${id}/status`, { status });
  },

  async deleteTask(id: number): Promise<void> {
    await api.delete(`/tasks/${id}`);
  },
};