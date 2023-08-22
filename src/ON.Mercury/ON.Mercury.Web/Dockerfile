# Use a Node.js base image
FROM node:18

# Set the working directory inside the container
WORKDIR /app

# Copy the package.json and yarn.lock files to the container
COPY package.json yarn.lock ./

# Install dependencies using Yarn
RUN yarn install

# Copy the rest of the project files to the container
COPY . .

# Expose the port that your application will run on
EXPOSE 3000

# Define the command to start your application
CMD ["yarn", "dev"]
