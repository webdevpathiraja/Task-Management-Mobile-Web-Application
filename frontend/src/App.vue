<template>
  <div class="min-h-screen bg-gray-50">
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-3 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center py-4 sm:py-6">
          <div class="font-sans">
            <div class="flex items-center justify-between">
              <div class="flex items-center space-x-1">
                <div class="z-10">
                  <img src="/logo.png" alt="Logo" class="h-16 w-10 object-cover" />
                </div>
                <div class="z-0">
                  <h1 class="text-lg sm:text-3xl font-semibold text-gray-800">PeoplesTASK</h1>
                </div>
              </div>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row space-y-2 sm:space-y-0 sm:space-x-4">
            <button
              @click="showUserForm = true"
              class="inline-flex items-center justify-center px-3 py-2 sm:px-4 border border-transparent text-xs sm:text-sm font-medium rounded-md text-white bg-limegreen hover:bg-kellygreen focus:outline-none focus:ring-2 focus:ring-kellygreen whitespace-nowrap"
            >
              <svg class="w-3 h-3 sm:w-4 sm:h-4 mr-1 sm:mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
              </svg>
              Add User
            </button>

           <button
              @click="showTaskForm = true"
              class="inline-flex items-center justify-center px-3 py-2 sm:px-4 border border-transparent text-xs sm:text-sm font-medium rounded-md text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-2 focus:ring-red-500 whitespace-nowrap"
            >
              <svg class="w-3 h-3 sm:w-4 sm:h-4 mr-1 sm:mr-2" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
              </svg>
              Add Task
            </button>

          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-3 sm:px-6 lg:px-8 py-4 sm:py-8">
      <!-- User Selector -->
      <UserSelector @user-changed="handleUserChanged" />
      
      <!-- Stats -->
      <div class="grid grid-cols-2 md:grid-cols-4 gap-3 sm:gap-6 mb-6 sm:mb-8">
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="p-3 sm:p-5">
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <div class="w-6 h-6 sm:w-8 sm:h-8 bg-blue-400 rounded-full flex items-center justify-center">
                  <svg class="w-3 h-3 sm:w-4 sm:h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v11a2 2 0 002 2h2m0-13h10a2 2 0 012 2v11a2 2 0 01-2 2H9m0-13v13"></path>
                  </svg>
                </div>
              </div>
              <div class="ml-3 sm:ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-xs sm:text-sm font-medium text-gray-500 truncate">Total Tasks</dt>
                  <dd class="text-sm sm:text-lg font-medium text-gray-900">{{ stats.total }}</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="p-3 sm:p-5">
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <div class="w-6 h-6 sm:w-8 sm:h-8 bg-red-600 rounded-full flex items-center justify-center">
                  <svg class="w-3 h-3 sm:w-4 sm:h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                  </svg>
                </div>
              </div>
              <div class="ml-3 sm:ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-xs sm:text-sm font-medium text-gray-500 truncate">Pending</dt>
                  <dd class="text-sm sm:text-lg font-medium text-gray-900">{{ stats.pending }}</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="p-3 sm:p-5">
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <div class="w-6 h-6 sm:w-8 sm:h-8 bg-yellow-400 rounded-full flex items-center justify-center">
                  <svg class="w-3 h-3 sm:w-4 sm:h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"></path>
                  </svg>
                </div>
              </div>
              <div class="ml-3 sm:ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-xs sm:text-sm font-medium text-gray-500 truncate">In Progress</dt>
                  <dd class="text-sm sm:text-lg font-medium text-gray-900">{{ stats.inProgress }}</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
        
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="p-3 sm:p-5">
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <div class="w-6 h-6 sm:w-8 sm:h-8 bg-limegreen rounded-full flex items-center justify-center">
                  <svg class="w-3 h-3 sm:w-4 sm:h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                  </svg>
                </div>
              </div>
              <div class="ml-3 sm:ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-xs sm:text-sm font-medium text-gray-500 truncate">Completed</dt>
                  <dd class="text-sm sm:text-lg font-medium text-gray-900">{{ stats.completed }}</dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Task List -->
      <div class="bg-white shadow rounded-lg p-3 sm:p-6">
        <h2 class="text-lg sm:text-xl font-semibold text-gray-900 mb-4 sm:mb-6 text-center">
          {{ selectedUserId ? 'User Tasks' : 'All Tasks' }}
        </h2>
        <TaskList
          :tasks="tasks"
          :loading="loading"
          @edit-task="handleEditTask"
          @delete-task="handleDeleteTask"
          @update-status="handleUpdateStatus"
        />
      </div>
    </main>

    <!-- Modals -->
    <TaskForm
      v-if="showTaskForm"
      :task="editingTask"
      @close="closeTaskForm"
      @submit="handleTaskSubmit"
    />
    
    <UserForm
      v-if="showUserForm"
      @close="showUserForm = false"
      @submit="handleUserSubmit"
    />

    <!-- Toast Notifications -->
    <div
      v-if="notification.show"
      class="fixed top-4 right-2 sm:right-4 z-50 max-w-xs sm:max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto"
      :class="notification.type === 'success' ? 'border-l-4 border-green-400' : 'border-l-4 border-red-400'"
    >
      <div class="p-3 sm:p-4">
        <div class="flex items-start">
          <div class="flex-shrink-0">
            <svg
              v-if="notification.type === 'success'"
              class="h-4 w-4 sm:h-5 sm:w-5 text-green-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
            </svg>
            <svg
              v-else
              class="h-4 w-4 sm:h-5 sm:w-5 text-red-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
          </div>
          <div class="ml-2 sm:ml-3 w-0 flex-1">
            <p class="text-xs sm:text-sm font-medium text-gray-900">{{ notification.message }}</p>
          </div>
          <div class="ml-3 sm:ml-4 flex-shrink-0 flex">
            <button
              @click="notification.show = false"
              class="bg-white rounded-md inline-flex text-gray-400 hover:text-gray-500 focus:outline-none"
            >
              <svg class="h-4 w-4 sm:h-5 sm:w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue';
import type { Task, CreateTaskRequest, UpdateTaskRequest, CreateUserRequest } from './types';
import { taskService, userService } from './services/api';
import UserSelector from './components/UserSelector.vue';
import TaskList from './components/TaskList.vue';
import TaskForm from './components/TaskForm.vue';
import UserForm from './components/UserForm.vue';

const tasks = ref<Task[]>([]);
const loading = ref(false);
const selectedUserId = ref<number | ''>('');
const showTaskForm = ref(false);
const showUserForm = ref(false);
const editingTask = ref<Task | undefined>();

const notification = reactive({
  show: false,
  message: '',
  type: 'success' as 'success' | 'error'
});

const stats = computed(() => {
  const total = tasks.value.length;
  const pending = tasks.value.filter(t => t.status === 'Pending').length;
  const inProgress = tasks.value.filter(t => t.status === 'In Progress').length;
  const completed = tasks.value.filter(t => t.status === 'Completed').length;
  
  return { total, pending, inProgress, completed };
});

const showNotification = (message: string, type: 'success' | 'error' = 'success') => {
  notification.message = message;
  notification.type = type;
  notification.show = true;
  
  setTimeout(() => {
    notification.show = false;
  }, 5000);
};

const loadTasks = async () => {
  loading.value = true;
  try {
    if (selectedUserId.value) {
      tasks.value = await taskService.getTasksByUserId(selectedUserId.value);
    } else {
      tasks.value = await taskService.getAllTasks();
    }
  } catch (error) {
    console.error('Failed to load tasks:', error);
    showNotification('Failed to load tasks', 'error');
  } finally {
    loading.value = false;
  }
};

const handleUserChanged = (userId: number | '') => {
  selectedUserId.value = userId;
  loadTasks();
};

const handleEditTask = (task: Task) => {
  editingTask.value = task;
  showTaskForm.value = true;
};

const handleDeleteTask = async (taskId: number) => {
  if (!confirm('Are you sure you want to delete this task?')) {
    return;
  }
  
  try {
    await taskService.deleteTask(taskId);
    showNotification('Task deleted successfully');
    loadTasks();
  } catch (error) {
    console.error('Failed to delete task:', error);
    showNotification('Failed to delete task', 'error');
  }
};

const handleUpdateStatus = async (taskId: number, status: string) => {
  try {
    await taskService.updateTaskStatus(taskId, status);
    showNotification('Task status updated successfully');
    loadTasks();
  } catch (error) {
    console.error('Failed to update task status:', error);
    showNotification('Failed to update task status', 'error');
  }
};

const handleTaskSubmit = async (data: CreateTaskRequest | UpdateTaskRequest) => {
  try {
    if (editingTask.value) {
      await taskService.updateTask(editingTask.value.id, data as UpdateTaskRequest);
      showNotification('Task updated successfully');
    } else {
      await taskService.createTask(data as CreateTaskRequest);
      showNotification('Task created successfully');
    }
    closeTaskForm();
    loadTasks();
  } catch (error) {
    console.error('Failed to save task:', error);
    showNotification('Failed to save task', 'error');
  }
};

const handleUserSubmit = async (data: CreateUserRequest) => {
  try {
    await userService.createUser(data);
    showNotification('User created successfully');
    showUserForm.value = false;
  } catch (error) {
    console.error('Failed to create user:', error);
    showNotification('Failed to create user', 'error');
  }
};

const closeTaskForm = () => {
  showTaskForm.value = false;
  editingTask.value = undefined;
};

onMounted(() => {
  loadTasks();
});
</script>