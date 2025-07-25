<template>
  <div class="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50" @click="closeModal">
    <div class="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white" @click.stop>
      <div class="mt-3">
        <h3 class="text-lg font-medium text-gray-900 mb-4">
          {{ isEditing ? 'Edit Task' : 'Create New Task' }}
        </h3>
        
        <form @submit.prevent="handleSubmit" class="space-y-4">
          <div>
            <label for="title" class="block text-sm font-medium text-gray-700 mb-1">
              Title *
            </label>
            <input
              id="title"
              v-model="form.title"
              type="text"
              required
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              :class="{ 'border-red-500': errors.title }"
            />
            <p v-if="errors.title" class="mt-1 text-sm text-red-600">{{ errors.title }}</p>
          </div>
          
          <div>
            <label for="description" class="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <textarea
              id="description"
              v-model="form.description"
              rows="3"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            ></textarea>
          </div>
          
          <div>
            <label for="status" class="block text-sm font-medium text-gray-700 mb-1">
              Status
            </label>
            <select
              id="status"
              v-model="form.status"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="Pending">Pending</option>
              <option value="In Progress">In Progress</option>
              <option value="Completed">Completed</option>
            </select>
          </div>
          
          <div v-if="!isEditing">
            <label for="userId" class="block text-sm font-medium text-gray-700 mb-1">
              Assign to User *
            </label>
            <select
              id="userId"
              v-model="form.userId"
              required
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              :class="{ 'border-red-500': errors.userId }"
            >
              <option value="">Select a user</option>
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.name }}
              </option>
            </select>
            <p v-if="errors.userId" class="mt-1 text-sm text-red-600">{{ errors.userId }}</p>
          </div>
          
          <div>
            <label for="dueDate" class="block text-sm font-medium text-gray-700 mb-1">
              Due Date
            </label>
            <input
              id="dueDate"
              v-model="form.dueDate"
              type="date"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            />
          </div>
          
          <div class="flex justify-end space-x-3 pt-4">
            <button
              type="button"
              @click="closeModal"
              class="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-200 border border-gray-300 rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500"
            >
              Cancel
            </button>
            <button
              type="submit"
              :disabled="submitting"
              class="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {{ submitting ? 'Saving...' : (isEditing ? 'Update' : 'Create') }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, watch } from 'vue';
import type { User, Task, CreateTaskRequest, UpdateTaskRequest } from '../types';
import { userService } from '../services/api';

const props = defineProps<{
  task?: Task;
}>();

const emit = defineEmits<{
  close: [];
  submit: [data: CreateTaskRequest | UpdateTaskRequest];
}>();

const users = ref<User[]>([]);
const submitting = ref(false);
const isEditing = ref(!!props.task);

const form = reactive({
  title: '',
  description: '',
  status: 'Pending' as 'Pending' | 'In Progress' | 'Completed',
  userId: 0,
  dueDate: ''
});

const errors = reactive({
  title: '',
  userId: ''
});

// Load users on mount
onMounted(async () => {
  try {
    users.value = await userService.getAllUsers();
  } catch (error) {
    console.error('Failed to load users:', error);
  }
});

// Populate form if editing
watch(() => props.task, (task) => {
  if (task) {
    form.title = task.title;
    form.description = task.description;
    form.status = task.status;
    form.userId = task.userId;
    form.dueDate = task.dueDate ? task.dueDate.split('T')[0] : '';
    isEditing.value = true;
  }
}, { immediate: true });

const validateForm = () => {
  errors.title = '';
  errors.userId = '';
  
  let isValid = true;
  
  if (!form.title.trim()) {
    errors.title = 'Title is required';
    isValid = false;
  }
  
  if (!isEditing.value && !form.userId) {
    errors.userId = 'Please select a user';
    isValid = false;
  }
  
  return isValid;
};

const handleSubmit = async () => {
  if (!validateForm()) {
    return;
  }
  
  submitting.value = true;
  
  try {
    const data = {
      title: form.title.trim(),
      description: form.description.trim(),
      status: form.status,
      ...(form.dueDate && { dueDate: form.dueDate }),
      ...(form.userId && { userId: form.userId })
    };
    
    emit('submit', data);
  } finally {
    submitting.value = false;
  }
};

const closeModal = () => {
  emit('close');
};
</script>