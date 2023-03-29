FROM jenkins/jenkins
USER root

# Install Docker
RUN apt-get update
RUN apt-get install curl -y
RUN curl -fsSL https://get.docker.com/ | sh
RUN usermod -aG docker jenkins

# Instal dotnet
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0

USER jenkins