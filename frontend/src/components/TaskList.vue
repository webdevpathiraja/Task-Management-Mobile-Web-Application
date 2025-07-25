<template>
  <div class="space-y-4">
    <div v-if="loading" class="text-center py-8">
      <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
      <p class="mt-2 text-gray-600">Loading tasks...</p>
    </div>
    
    <div v-else-if="tasks.length === 0" class="text-center py-8 text-gray-500">
      <p>No tasks found. Create your first task!</p>
    </div>
    
    <div v-else class="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      <div
        v-for="task in tasks"
        :key="task.id"
        class="bg-white rounded-lg shadow-md p-6 border-l-4"
        :class="getTaskBorderColor(task.status)"
      >
        <div class="flex justify-between items-start mb-3">
          <h3 class="text-lg font-semibold text-gray-900 truncate">{{ task.title }}</h3>
          <div class="flex space-x-2">
            <button
              @click="$emit('editTask', task)"
              class="text-blue-600 hover:text-blue-800 transition-colors"
              title="Edit task"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
              </svg>
            </button>
            <button
              @click="$emit('deleteTask', task.id)"
              class="text-red-600 hover:text-red-800 transition-colors"
              title="Delete task"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
              </svg>
            </button>
          </div>
        </div>
        
        <p class="text-gray-600 text-sm mb-3 line-clamp-2">{{ task.description }}</p>
        
        <div class="flex items-center justify-between mb-3">
          <span
            class="px-2 py-1 text-xs font-medium rounded-full"
            :class="getStatusColor(task.status)"
          >
            {{ task.status }}
          </span>
          <span v-if="task.userName" class="text-xs text-gray-500">
            {{ task.userName }}
          </span>
        </div>
        
        <div class="flex justify-between items-center text-xs text-gray-500">
          <span>Created: {{ formatDate(task.createdDate) }}</span>
          <span v-if="task.dueDate" class="text-red-600">
            Due: {{ formatDate(task.dueDate) }}
          </span>
        </div>
        
        <div class="mt-4 flex space-x-2">
          <button
            v-for="status in ['Pending', 'In Progress', 'Completed']"
            :key="status"
            @click="$emit('updateStatus', task.id, status)"
            :disabled="task.status === status"
            class="flex-1 px-3 py-1 text-xs font-medium rounded transition-colors"
            :class="task.status === status 
              ? 'bg-gray-200 text-gray-500 cursor-not-allowed' 
              : 'bg-gray-100 text-gray-700 hover:bg-gray-200'"
          >
            {{ status }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Task } from '../types';

defineProps<{
  tasks: Task[];
  loading: boolean;
}>();

defineEmits<{
  editTask: [task: Task];
  deleteTask: [taskId: number];
  updateStatus: [taskId: number, status: string];
}>();

const getTaskBorderColor = (status: string) => {
  switch (status) {
    case 'Pending':
      return 'border-red-600';
    case 'In Progress':
      return 'border-yellow-400';
    case 'Completed':
      return 'border-limegreen';
    default:
      return 'border-gray-400';
  }
};

const getStatusColor = (status: string) => {
  switch (status) {
    case 'Pending':
      return 'bg-red-100 text-red-600';
    case 'In Progress':
      return 'bg-yellow-100 text-yellow-500';
    case 'Completed':
      return 'bg-green-100 text-green-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString();
};
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>