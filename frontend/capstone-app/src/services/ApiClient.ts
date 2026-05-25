import axios, { AxiosInstance } from 'axios';
import { User, Project, Task, CreateProjectRequest, CreateTaskRequest } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

  // User endpoints
  async getUsers(): Promise<User[]> {
    const response = await this.client.get('/users');
    return response.data;
  }

  async getUser(id: string): Promise<User> {
    const response = await this.client.get(`/users/${id}`);
    return response.data;
  }

  async loginUser(email: string, password: string): Promise<User> {
    const response = await this.client.post('/users/login', { email, password });
    return response.data.user;
  }

  // Project endpoints
  async getProjects(): Promise<Project[]> {
    const response = await this.client.get('/projects');
    return response.data;
  }

  async getProject(id: string): Promise<Project> {
    const response = await this.client.get(`/projects/${id}`);
    return response.data;
  }

  async getUserProjects(userId: string): Promise<Project[]> {
    const response = await this.client.get(`/projects/user/${userId}`);
    return response.data;
  }

  async createProject(userId: string, project: CreateProjectRequest): Promise<Project> {
    const response = await this.client.post(`/projects?userId=${userId}`, project);
    return response.data;
  }

  async updateProject(id: string, project: CreateProjectRequest): Promise<Project> {
    const response = await this.client.put(`/projects/${id}`, project);
    return response.data;
  }

  async deleteProject(id: string): Promise<void> {
    await this.client.delete(`/projects/${id}`);
  }

  // Task endpoints
  async getProjectTasks(projectId: string): Promise<Task[]> {
    const response = await this.client.get(`/projects/${projectId}/tasks`);
    return response.data;
  }

  async getTask(projectId: string, taskId: string): Promise<Task> {
    const response = await this.client.get(`/projects/${projectId}/tasks/${taskId}`);
    return response.data;
  }

  async createTask(projectId: string, task: CreateTaskRequest): Promise<Task> {
    const response = await this.client.post(`/projects/${projectId}/tasks`, task);
    return response.data;
  }

  async updateTask(projectId: string, taskId: string, task: CreateTaskRequest): Promise<Task> {
    const response = await this.client.put(`/projects/${projectId}/tasks/${taskId}`, task);
    return response.data;
  }

  async deleteTask(projectId: string, taskId: string): Promise<void> {
    await this.client.delete(`/projects/${projectId}/tasks/${taskId}`);
  }
}

export default new ApiClient();
